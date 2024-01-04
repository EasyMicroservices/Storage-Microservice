using EasyMicroservices.ContentsMicroservice.Helpers;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.FileManager.Interfaces;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.StorageMicroservice.Contracts;
using EasyMicroservices.StorageMicroservice.Contracts.Requests;
using EasyMicroservices.StorageMicroservice.Database.Entities;
using EasyMicroservices.StorageMicroservice.Logics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyMicroservices.StorageMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileController : SimpleQueryServiceController<FileEntity, AddFileRequestContract, FileContract, FileContract, long>
    {
        private readonly IDirectoryManagerProvider _directoryManagerProvider;
        private readonly IFileManagerProvider _fileManagerProvider;
        private readonly IAppUnitOfWork _unitOfWork;
        public FileController(IAppUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _directoryManagerProvider = unitOfWork.GetDirectoryManagerProvider();
            _fileManagerProvider = unitOfWork.GetFileManagerProvider();
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<MessageContract<FileContract>> UploadFile([FromForm] AddFileRequestContract input)
        {
            var fileExtension = Path.GetExtension(input.File.FileName);

            var filePassword = Guid.NewGuid().ToString();
            var fileName = $"{input.File.FileName}";

            var newFile = new FileEntity
            {
                CreationDateTime = DateTime.Now,
                Name = input.File.FileName,
                ContentType = input.File.ContentType,
                Length = input.File.Length,
                Extension = fileExtension,
                FolderId = input.FolderId ?? 1,
                Password = filePassword,
                UniqueIdentity = input.UniqueIdentity,
                Key = input.Key
            };
            var logic = _unitOfWork.GetLongLogic<FileEntity>();
            var result = await logic.AddEntity(newFile);

            result.Result.Path = await FileLogic.NameToFullPath($"{result.Result.Id}_{fileName}", _unitOfWork);
            await logic.SaveChangesAsync();

            using var stream = new FileStream(newFile.Path, FileMode.Create);
            await input.File.CopyToAsync(stream);


            if (result.TryGetResult(out FileEntity file, out MessageContract<FileContract> errorContract))
            {
                return new FileContract
                {
                    Id = file.Id,
                    CreationDateTime = newFile.CreationDateTime,
                    Name = newFile.Name,
                    ContentType = newFile.ContentType,
                    Length = newFile.Length,
                    Extension = newFile.Extension,
                    FolderId = newFile.FolderId,
                    Password = newFile.Password,
                    Path = newFile.Path,
                    DownloadLink = GenerateDownloadLink(HttpContext, newFile.Id, newFile.Password),
                    UniqueIdentity = file.UniqueIdentity,
                    Key = file.Key
                };
            }
            return errorContract;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<MessageContract<FileContract>> GetByIdAndPassword(GetByIdAndPasswordRequestContract request, CancellationToken cancellationToken = default)
        {
            return _unitOfWork.GetLongContractLogic<FileEntity, FileContract>()
                .GetBy(x => x.Id == request.Id && x.Password == request.Password, cancellationToken: cancellationToken);
        }

        [HttpPost]
        public async Task<MessageContract<FileContract>> GetByKeyAndUniqueIdentity(GetByKeyRequestContract request, CancellationToken cancellationToken = default)
        {
            return await ContractLogic.GetByUniqueIdentity(request,
                Cores.DataTypes.GetUniqueIdentityType.All,
                q => q.Where(x => x.Key == request.Key),
                cancellationToken);
        }

        [HttpPost]
        public async Task<ListMessageContract<FileContract>> GetAllByKeyAndUniqueIdentity(GetByKeyRequestContract request, CancellationToken cancellationToken = default)
        {
            return await ContractLogic.GetAllByUniqueIdentity(request,
                Cores.DataTypes.GetUniqueIdentityType.All,
                q => q.Where(x => x.Key == request.Key),
                cancellationToken);
        }

        [HttpPost]
        public async Task<MessageContract<FileContract>> UploadOrReplaceFile([FromForm] AddFileRequestContract input)
        {
            var currentFile = await base.GetAllByUniqueIdentity(input.UniqueIdentity);
            var find = currentFile.GetCheckedResult().FirstOrDefault(x => x.Key == input.Key);
            if (currentFile.HasItems && find != null)
            {
                var deleteResult = await DeleteFileByPassword(find.Id, find.Password);
                if (!deleteResult)
                    return deleteResult.ToContract<FileContract>();
            }
            return await UploadFile(input);
        }

        [HttpDelete]
        public async Task<MessageContract> DeleteFileByPassword(long fileId, string password)
        {
            var find = await GetById(new Cores.Contracts.Requests.GetByIdRequestContract<long>()
            {
                Id = fileId
            });
            if (find)
            {
                if (find.Result.Password == password)
                {
                    var filePath = await FileLogic.NameToFullPath(find.Result.Path, _unitOfWork);
                    if (await _fileManagerProvider.IsExistFileAsync(filePath))
                    {
                        await _fileManagerProvider.DeleteFileAsync(filePath);
                    }
                    var deleteResult = await SoftDeleteById(new Cores.Contracts.Requests.SoftDeleteRequestContract<long>
                    {
                        Id = fileId,
                        IsDelete = true
                    });
                    return deleteResult;
                }
                else
                    return (FailedReasonType.ValidationsError, "File id or Password is not valid");
            }
            return find;
        }

        private static string GenerateDownloadLink(HttpContext httpContext, long fileId, string password)
        {
            string DownloadLink = @$"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/File/DownloadFile?id={fileId}&password={password}";
            return DownloadLink;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public async Task<FileContentResult> DownloadFileAsync([FromQuery] long id, [FromQuery] string password)
        {
            var file = await GetById(new Cores.Contracts.Requests.GetByIdRequestContract<long>()
            {
                Id = id
            });
            if (!file || file.Result.Password != password)
            {
                HttpContext.Response.StatusCode = 404;
                Console.WriteLine($"File not found: {file.Result?.Id} {file.Result?.Password}");
                return default;
            }
            else
            {
                var filePath = await FileLogic.NameToFullPath(file.Result.Path, _unitOfWork);

                if (!await _fileManagerProvider.IsExistFileAsync(filePath))
                {
                    HttpContext.Response.StatusCode = 404;
                    Console.WriteLine($"File not found: {filePath}");
                    return default;
                }

                var fileBytes = await _fileManagerProvider.ReadAllBytesAsync(filePath);

                return File(fileBytes, file.Result.ContentType, file.Result.Name);
            }
        }
    }
}