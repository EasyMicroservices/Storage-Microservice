using EasyMicroservices.ContentsMicroservice.Helpers;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.FileManager.Interfaces;
using EasyMicroservices.ServiceContracts;
using EasyMicroservices.StorageMicroservice.Contracts;
using EasyMicroservices.StorageMicroservice.Database.Contexts;
using EasyMicroservices.StorageMicroservice.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace EasyMicroservices.StorageMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FolderController : SimpleQueryServiceController<FolderEntity, AddFolderContract, UpdateFolderContract, FolderContract, long>
    {
        protected readonly IDirectoryManagerProvider _directoryManagerProvider;
        protected readonly IFileManagerProvider _fileManagerProvider;

        private readonly IContractLogic<FolderEntity, AddFolderContract, UpdateFolderContract, FolderContract, long> _contractLogic;

        readonly IAppUnitOfWork unitOfWork;
        public FolderController(IAppUnitOfWork _unitOfWork) : base(_unitOfWork) 
        {
            unitOfWork = _unitOfWork;
            _directoryManagerProvider = _unitOfWork.GetDirectoryManagerProvider();
            _fileManagerProvider = _unitOfWork.GetFileManagerProvider();
            _contractLogic = _unitOfWork.GetContractLogic<FolderEntity, AddFolderContract, UpdateFolderContract, FolderContract, long>();
        }

        private string PathToFullPath(string name)
        {
            string webRootPath = @Directory.GetCurrentDirectory();
            string directoryPath = _directoryManagerProvider.PathProvider.Combine(webRootPath, name);
            return directoryPath;
        }

        //public override Task<MessageContract<long>> Add(AddFolderContract request, CancellationToken cancellationToken = default)
        //{
        //    return base.Add(request, cancellationToken);
        //}


        //[HttpGet]
        //public async Task<MessageContract<List<FolderContract>>> GetAll()
        //{
        //    var Folders = await _context.Folders.ToListAsync();

        //    if (Folders.Count > 0)
        //    {
        //        return Folders.Select(f => new FolderContract
        //        {
        //            Id = f.Id,
        //            CreationDateTime = f.CreationDateTime,
        //            Name = f.Name,
        //            Path = f.Path
        //        }).ToList();
        //    }
        //    else
        //        return (FailedReasonType.NotFound, "No directory found");
        //}

        //[HttpPut]
        //public async Task<MessageContract<FolderContract>> UpdateAsync(UpdateFolderContract input)
        //{
        //    var result = new MessageContract<FolderContract>();
        //    string webRootPath = @Directory.GetCurrentDirectory();
        //    string Path = input.Path.ToLower();

        //    if (Regex.IsMatch(Path, @"^[a-zA-Z]+$"))
        //    {

        //        var existingFolder = await _context.Folders.FindAsync(input.Id);

        //        if (existingFolder != null && _context.Folders.Any(folder => folder.Path == existingFolder.Path))
        //        {
        //            Directory.Move(PathToFullPath(existingFolder.Path), PathToFullPath(input.Path));


        //            existingFolder.ModificationDateTime = DateTime.Now;
        //            existingFolder.Name = input.Name ?? existingFolder.Name;
        //            existingFolder.Path = Path ?? existingFolder.Path;

        //            result.Result = new FolderContract
        //            {
        //                Id = existingFolder.Id,
        //                CreationDateTime = existingFolder.CreationDateTime,
        //                Name = existingFolder.Name,
        //                Path = existingFolder.Path,
        //            };
        //            result.IsSuccess = true;

        //            await _context.SaveChangesAsync();

        //        }
        //        else
        //        {
        //            result.IsSuccess = false;
        //            result.Error.Message = "Folder doesn't exist";
        //        }
        //    }
        //    else
        //    {
        //        result.IsSuccess = false;
        //        result.Error.Message = "Your folder path isn't valid.";
        //    }

        //    return result;
        //}

        //[HttpPost]
        //public async Task<MessageContract<FolderContract>> AddAsync(AddFolderContract input)
        //{
        //    var result = new MessageContract<FolderContract>();

        //    string Path = input.Path.ToLower();

        //    if (Regex.IsMatch(Path, @"^[a-zA-Z]+$"))
        //    {

        //        bool exists = await _directoryManagerProvider.IsExistDirectoryAsync(PathToFullPath(Path)) || _context.Folders.Any(folder => folder.Path == Path); ;

        //        if (!exists)
        //        {

        //            var Folder = new FolderEntity
        //            {
        //                CreationDateTime = DateTime.Now,
        //                Name = input.Name,
        //                Path = Path,
        //            };

        //            var AddedFolder = await _context.Folders.AddAsync(Folder);
        //            await _context.SaveChangesAsync();

        //            await _directoryManagerProvider.CreateDirectoryAsync(PathToFullPath(Path));
        //            await _fileManagerProvider.CreateFileAsync(_directoryManagerProvider.PathProvider.Combine(PathToFullPath(Path), ".gitkeep"));

        //            result.Result = new FolderContract
        //            {
        //                Id = AddedFolder.Entity.Id,
        //                CreationDateTime = Folder.CreationDateTime,
        //                Name = Folder.Name,
        //                Path = Folder.Path,
        //            };
        //            result.IsSuccess = true;
        //        }
        //        else
        //        {
        //            result.IsSuccess = false;
        //            result.Error.Message = "Folder already exists";
        //        }
        //    }
        //    else
        //    {
        //        result.IsSuccess = false;
        //        result.Error.Message = "Your folder path isn't valid.";
        //    }

        //    return result;
        //}

        //[HttpDelete]
        //public async Task<MessageContract> DeleteAsync(long fileId)
        //{
        //    var result = new MessageContract();

        //    var entityToDelete = await _context.Folders.FindAsync(fileId);

        //    if (entityToDelete != null)
        //    {

        //        var FilesInFolder = _context.Files.Where(o => o.FolderId == entityToDelete.Id);
        //        if (FilesInFolder.Any())
        //        {
        //            foreach (var file in FilesInFolder)
        //            {
        //                _context.Files.Remove(file);
        //            }
        //        }

        //        await _directoryManagerProvider.DeleteDirectoryAsync(PathToFullPath(entityToDelete.Path), true);

        //        _context.Folders.Remove(entityToDelete);

        //        await _context.SaveChangesAsync();

        //        result.IsSuccess = true;
        //    }
        //    else
        //    {
        //        result.IsSuccess = false;
        //        result.Error.Message = "Folder not found";
        //    }

        //    return result;

        //}

        //[HttpPost]
        //public async Task<Result> AddSingleFileAsync(List<IFormFile> files)
        //{


        //    //long size = files.Sum(f => f.Length);

        //    //foreach (var formFile in files)
        //    //{
        //    //    if (formFile.Length > 0)
        //    //    {
        //    //        var filePath = Path.GetTempFileName();

        //    //        using (var stream = System.IO.File.Create(filePath))
        //    //        {
        //    //            await formFile.CopyToAsync(stream);
        //    //        }
        //    //    }
        //    //}

        //    //// Process uploaded files
        //    //// Don't rely on or trust the FileName property without validation.

        //    //return Ok(new { count = files.Count, size });
        //}

    }
}