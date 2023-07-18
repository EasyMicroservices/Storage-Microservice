using System.Configuration;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.StorageMicroservice.Database.Entities;
using EasyMicroservices.StorageMicroservice.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using EasyMicroservices.StorageMicroservice.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using EasyMicroservices.FileManager.Interfaces;
using ServiceContracts;

namespace EasyMicroservices.StorageMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FolderController : ControllerBase
    {
        protected readonly IDirectoryManagerProvider _directoryManagerProvider;
        protected readonly IFileManagerProvider _fileManagerProvider;
        private readonly StorageContext _context;
        
        public FolderController(StorageContext context, IDirectoryManagerProvider directoryManagerProvider, IFileManagerProvider fileManagerProvider)
        {
            _directoryManagerProvider = directoryManagerProvider;
            _fileManagerProvider = fileManagerProvider;
            _context = context;
        }

        private string PathToFullPath(string name)
        {
            string webRootPath = @Directory.GetCurrentDirectory();
            string directoryPath = _directoryManagerProvider.PathProvider.Combine(webRootPath, "wwwroot", Constants.RootAddress, name);
            return directoryPath ?? _directoryManagerProvider.PathProvider.Combine(webRootPath, "wwwroot", Constants.RootAddress);
        }

        [HttpGet]
        public async Task<MessageContract<List<FolderEntity>>> GetAll()
        {
            var result = new MessageContract<List<FolderEntity>>();
            var Folders = await _context.Folders.ToListAsync();


            if (Folders.Count > 0)
            {
                result.IsSuccess = true;
                result.Result = Folders;
            }
            else
            {
                result.IsSuccess = false;
                result.Error.Message = "No directory found";
            }

            return result;
        }

        [HttpPut]
        public async Task<MessageContract<FolderEntity>> UpdateAsync(UpdateFolderContract input)
        {
            var result = new MessageContract<FolderEntity>();
            string webRootPath = @Directory.GetCurrentDirectory();
            string Path = input.Path.ToLower();

            if (Regex.IsMatch(Path, @"^[a-zA-Z]+$"))
            {

                var existingFolder = await _context.Folders.FindAsync(input.Id);

                if (existingFolder != null && _context.Folders.Any(folder => folder.Path == existingFolder.Path))
                {
                    Directory.Move(PathToFullPath(existingFolder.Path), PathToFullPath(input.Path));


                    existingFolder.ModificationDateTime = DateTime.Now;
                    existingFolder.Name = input.Name ?? existingFolder.Name;
                    existingFolder.Path = Path ?? existingFolder.Path;

                    result.Result = existingFolder;
                    result.IsSuccess = true;

                    await _context.SaveChangesAsync();

                }
                else
                {
                    result.IsSuccess = false;
                    result.Error.Message = "Folder doesn't exist";
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Error.Message = "Your folder path isn't valid.";
            }

            return result;
        }

        [HttpPost]
        public async Task<MessageContract<FolderEntity>> AddAsync(AddFolderContract input)
        {
            var result = new MessageContract<FolderEntity>();
            
            string Path = input.Path.ToLower();

            if (Regex.IsMatch(Path, @"^[a-zA-Z]+$"))
            {

                bool exists = await _directoryManagerProvider.IsExistDirectoryAsync(PathToFullPath(Path)) || _context.Folders.Any(folder => folder.Path == Path); ;

                if (!exists)
                {

                    var folder = new FolderEntity
                    {
                        CreationDateTime = DateTime.Now,
                        Name = input.Name,
                        Path = Path,
                    };

                    var AddedFolder = await _context.Folders.AddAsync(folder);
                    await _context.SaveChangesAsync();

                    await _directoryManagerProvider.CreateDirectoryAsync(PathToFullPath(Path));
                    await _fileManagerProvider.CreateFileAsync(_directoryManagerProvider.PathProvider.Combine(PathToFullPath(Path), ".gitkeep"));

                    result.Result = AddedFolder.Entity;
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Error.Message = "Folder already exists";
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Error.Message = "Your folder path isn't valid.";
            }

            return result;
        }

        [HttpDelete]
        public async Task<MessageContract> DeleteAsync(long Id)
        {
            var result = new MessageContract();

            var entityToDelete = await _context.Folders.FindAsync(Id);

            if (entityToDelete != null)
            {
                
                var FilesInFolder = _context.Files.Where(o => o.FolderId == entityToDelete.Id);
                if (FilesInFolder.Any())
                {
                    foreach (var file in FilesInFolder)
                    {
                        _context.Files.Remove(file);
                    }
                }
                
                await _directoryManagerProvider.DeleteDirectoryAsync(PathToFullPath(entityToDelete.Path), true);

                _context.Folders.Remove(entityToDelete);

                await _context.SaveChangesAsync();

                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Error.Message = "Folder not found";
            }

            return result;

        }

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