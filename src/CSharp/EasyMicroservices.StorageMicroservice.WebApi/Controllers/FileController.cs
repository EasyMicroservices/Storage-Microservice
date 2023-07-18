using System.Configuration;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.StorageMicroservice.Database.Entities;
using EasyMicroservices.StorageMicroservice.Contracts;
using Microsoft.AspNetCore.Mvc;
using EasyMicroservices.StorageMicroservice.Database.Contexts;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using EasyMicroservices.FileManager.Interfaces;
using ServiceContracts;

namespace EasyMicroservices.StorageMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileController : ControllerBase
    {

        protected readonly IDirectoryManagerProvider _directoryManagerProvider;
        private readonly IFileManagerProvider _fileManagerProvider;
        private readonly StorageContext _context;

        public FileController(StorageContext context, IDirectoryManagerProvider directoryManagerProvider, IFileManagerProvider fileManagerProvider)
        {
            _directoryManagerProvider = directoryManagerProvider;
            _fileManagerProvider = fileManagerProvider;
            _context = context;
        }

        private string NameToFullPath(string FilePath)
        {
            string webRootPath = @Directory.GetCurrentDirectory();
            string directoryPath = _directoryManagerProvider.PathProvider.Combine(webRootPath, "wwwroot", Constants.RootAddress, FilePath); 
            return directoryPath;
        }



        [HttpPost("AddFileAsync")]
        public async Task<MessageContract<object>> AddSingleFileAsync([FromForm] AddFileContract input)
        {

            var result = new MessageContract<object>
            {
                IsSuccess = true
            };

            if (input.File == null || input.File.Length == 0)
            {
                result.IsSuccess = false;
                result.Error.Message = "File is empty.";
            } else
            {
                var FileExtension = Path.GetExtension(input.File.FileName);

                if (!Constants.AllowedFileExtensions.Contains(FileExtension))
                {
                    result.IsSuccess = false;
                    result.Error.Message = $"The {FileExtension} file type is not valid. It must be:\n{string.Join(' ', Constants.AllowedFileExtensions)}";
                } else
                {
                    var GuId = Guid.NewGuid();

                    bool isGuIdUnique = _context.Files.Where(f => f.Guid.ToString() == GuId.ToString()).Count() > 0;
                    
                    while (isGuIdUnique)
                    {
                        GuId = Guid.NewGuid();
                        isGuIdUnique = _context.Files.Where(f => f.Guid.ToString() == GuId.ToString()).Count() > 0;
                    }

                    var FileName = $"{GuId}{FileExtension}";

                    FolderEntity Folder = new();

                    if (input.FolderId == null || input.FolderId == 0)
                    {
                        Folder = _context.Folders.Where(o => o.Id == 1).FirstOrDefault();
                    } else
                    {
                        Folder = _context.Folders.Where(o => o.Id == input.FolderId).FirstOrDefault();
                    }

                    if (Folder != null)
                    {

                        var newFile = new FileEntity {
                            CreationDateTime = DateTime.Now,
                            Name = input.File.FileName ?? "default",
                            Guid = GuId,
                            ContentType = input.File.ContentType ?? "text/plain",
                            Length = input.File.Length,
                            Extension = FileExtension,
                            FolderId = Folder.Id,
                            Password = input.Password ?? null,
                            Path = Folder.Id == 1 ? Path.Combine(FileName) : Path.Combine(Folder.Path, FileName)
                        };

                        using var stream = new FileStream(NameToFullPath(newFile.Path), FileMode.Create);
                        await input.File.CopyToAsync(stream);

                        await _context.Files.AddAsync(newFile);
                        await _context.SaveChangesAsync();

                        result.Result = new
                        {
                            newFile.CreationDateTime,
                            newFile.Name,
                            newFile.Guid,
                            newFile.ContentType,
                            newFile.Length,
                            newFile.Extension,
                            newFile.FolderId,
                            newFile.Password,
                            newFile.Path,
                            DownloadLink = GenerateDownloadLink(HttpContext, newFile.Guid.ToString(), newFile.Password),
                        };

                    } else
                    {
                        result.IsSuccess = false;
                        result.Error.Message = $"FolderId must not be empty or unavailable when Root folder is not available in database";
                    }


                }

            }

            return result;

        }

        [HttpDelete]
        public async Task<MessageContract<FileEntity>> DeleteFileByGuidAsync(string guid, string? password)
        {
            var Result = new MessageContract<FileEntity>
            {
                IsSuccess = true
            };

            string Password = password ?? string.Empty;
            var file = _context.Files.Where(o => o.Guid.ToString() == guid).FirstOrDefault();

            if (file == null)
            {
                Result.IsSuccess = false;
                Result.Error.Message = "File not found";
            }
            else
            {
                if (file.Password == password)
                {
                    var filePath = NameToFullPath(file.Path);
                    if (await _fileManagerProvider.IsExistFileAsync(filePath))
                    {
                        await _fileManagerProvider.DeleteFileAsync(filePath);

                        _context.Files.Remove(file);
                        await _context.SaveChangesAsync();

                        Result.Result = file;
                    }
                }
                else
                {
                    Result.IsSuccess = false;
                    Result.Error.Message = "Password is not correct";
                }
            }

            return Result;

        }


        [HttpDelete]
        public async Task<MessageContract<FileEntity>> DeleteFileByIdAsync(long id, string? password)
        {
            var file = _context.Files.Where(o => o.Id == id).FirstOrDefault();

            return await DeleteFileByGuidAsync(file.Guid.ToString(), password);
        }

        private static string GenerateDownloadLink(HttpContext httpContext, string fileGuid, string Password)
        {
            string DownloadLink = @$"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/File/DownloadFile/{fileGuid}" + $"?password={Password ?? string.Empty}";
            return DownloadLink;
        }

        [HttpGet("{fileGuid}")]
        public async Task<IActionResult> DownloadFile(string fileGuid, [FromQuery] string? password)
        {
            var file = _context.Files.Where(o => o.Guid.ToString() == fileGuid).FirstOrDefault();
            if (file == null)
            {
                return NotFound();
            } else
            {
                if(file.Password == password)
                {
                    var filePath = NameToFullPath(file.Path);

                    if (!await _fileManagerProvider.IsExistFileAsync(filePath))
                        return NotFound();
                    
                    var fileBytes = await _fileManagerProvider.ReadAllBytesAsync(filePath);

                    return File(fileBytes, "application/octet-stream", file.Name);
                } else
                {
                    return Unauthorized();
                }
            }

        }

        [HttpGet("{FileId}")]
        public async Task<IActionResult> DownloadFileWithIdAsync(long FileId, [FromQuery] string? password)
        {
            var file = _context.Files.Where(o => o.Id == FileId).FirstOrDefault();

            return await DownloadFile(file.Guid.ToString(), password);
        }


        //[HttpGet]
        //public ResultContract GetFilesByFolderId(long FolderId)
        //{
        //    var Result = new ResultContract
        //    {
        //        IsSuccessful = true
        //    };

        //    if (FolderId != 0)
        //    {
        //        var Files = _context.Files.Where(o => o.FolderId == FolderId);
        //        if (!Files.Any())
        //        {
        //            Result.IsSuccessful = false;
        //            Result.Message = "There isn't any file in entered Folder.";
        //        }
        //        else
        //        {

        //            Result.OutputRes = Files.Select(f => new {
        //                f.Id,
        //                f.Name,
        //                f.Guid,
        //                f.Length,
        //                f.FolderId,
        //                f.Extension,
        //                f.ContentType,
        //                f.CreationDateTime,
        //                f.ModificationDateTime,
        //                DownloadLink = GenerateDownloadLink(HttpContext, f.Guid, f.Password),
        //                Folder = new
        //                {
        //                    f.Folder.Id,
        //                    f.Folder.Name
        //                },
        //            }).ToList();
        //        }
        //    }

        //    return Result;
        //}



    }
}