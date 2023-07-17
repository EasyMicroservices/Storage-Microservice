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

namespace EasyMicroservices.StorageMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileController : ControllerBase
    {

        private readonly StorageContext _context;

        public FileController(StorageContext context)
        {
            _context = context;
        }

        private string NameToFullPath(string FilePath)
        {
            string webRootPath = @Directory.GetCurrentDirectory();
            string directoryPath = Path.Combine(webRootPath, "wwwroot", Constants.RootAddress, FilePath); 
            return directoryPath;
        }



        [HttpPost("AddFileAsync")]
        public async Task<ResultContract> AddSingleFileAsync([FromForm] AddFileDom input)
        {

            ResultContract Result = new();
            Result.IsSuccessful = true;

            if (input.File == null || input.File.Length == 0)
            {
                Result.IsSuccessful = false;
                Result.Message = "File is empty.";
            } else
            {
                var FileExtension = Path.GetExtension(input.File.FileName);

                if (!Constants.AllowedFileExtensions.Contains(FileExtension))
                {
                    Result.IsSuccessful = false;
                    Result.Message = $"The {FileExtension} file type is not valid. It must be:\n{string.Join(' ', Constants.AllowedFileExtensions)}";
                } else
                {
                    var GuId = Guid.NewGuid();

                    bool isGuIdUnique = _context.Files.Where(f => f.Guid == GuId.ToString()).Count() > 0;
                    
                    while (isGuIdUnique)
                    {
                        GuId = Guid.NewGuid();
                        isGuIdUnique = _context.Files.Where(f => f.Guid == GuId.ToString()).Count() > 0;
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
                            Guid = GuId.ToString(),
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

                        Result.Message = "File uploaded successfully.";
                        Result.OutputRes = new
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
                            DownloadLink = GenerateDownloadLink(HttpContext, newFile.Guid, newFile.Password),
                        };

                    } else
                    {
                        Result.IsSuccessful = false;
                        Result.Message = $"FolderId must not be empty or unavailable when Root folder is not available in database";
                    }


                }

            }

            return Result;

        }

        [HttpDelete]
        public async Task<ResultContract> DeleteFileByGuidAsync(string guid, string? password)
        {
            var Result = new ResultContract();
            Result.IsSuccessful = true;

            string Password = password ?? string.Empty;
            var file = _context.Files.Where(o => o.Guid == guid).FirstOrDefault();

            if (file == null)
            {
                Result.IsSuccessful = false;
                Result.Message = "File not found";
            }
            else
            {
                if (file.Password == password)
                {
                    var filePath = NameToFullPath(file.Path);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);

                        _context.Files.Remove(file);
                        await _context.SaveChangesAsync();

                        Result.OutputRes = file;
                        Result.Message = "Sucessfully deleted.";
                    }
                }
                else
                {
                    Result.IsSuccessful = false;
                    Result.Message = "Password is not correct";
                }
            }

            return Result;

        }


        [HttpDelete]
        public async Task<ResultContract> DeleteFileByIdAsync(long id, string? password)
        {
            var file = _context.Files.Where(o => o.Id == id).FirstOrDefault();

            return await DeleteFileByGuidAsync(file.Guid, password);
        }

        private static string GenerateDownloadLink(HttpContext httpContext, string fileGuid, string Password)
        {
            string DownloadLink = @$"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/File/DownloadFile/{fileGuid}" + $"?password={Password ?? string.Empty}";
            return DownloadLink;
        }

        [HttpGet("{fileGuid}")]
        public IActionResult DownloadFile(string fileGuid, [FromQuery] string? password)
        {
            var file = _context.Files.Where(o => o.Guid == fileGuid).FirstOrDefault();
            if (file == null)
            {
                return NotFound();
            } else
            {
                if(file.Password == password)
                {
                    var filePath = NameToFullPath(file.Path);

                    if (!System.IO.File.Exists(filePath))
                        return NotFound();

                    var fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, "application/octet-stream", file.Name);
                } else
                {
                    return Unauthorized();
                }
            }

        }

        [HttpGet("{FileId}")]
        public IActionResult DownloadFileWithId(long FileId, [FromQuery] string? password)
        {
            var file = _context.Files.Where(o => o.Id == FileId).FirstOrDefault();

            return DownloadFile(file.Guid, password);
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