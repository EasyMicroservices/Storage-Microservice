using System.Configuration;
using EasyMicroservices.Cores.AspCoreApi;
using EasyMicroservices.Cores.Database.Interfaces;
using EasyMicroservices.StorageMicroservice.Database.Entities;
using EasyMicroservices.StorageMicroservice.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using EasyMicroservices.StorageMicroservice.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EasyMicroservices.StorageMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FolderController : ControllerBase
    {
        private readonly StorageContext _context;


        public FolderController(StorageContext context)
        {
            _context = context;
        }

        private string PathToFullPath(string name)
        {
            string webRootPath = @Directory.GetCurrentDirectory();
            string directoryPath = Path.Combine(webRootPath, "wwwroot", Constants.RootAddress, name);
            return directoryPath ?? Path.Combine(webRootPath, "wwwroot", Constants.RootAddress);
        }

        [HttpGet]
        public async Task<ResultContract> GetAll()
        {
            ResultContract result = new();
            var Folders = await _context.Folders.ToListAsync();


            if (Folders.Count > 0)
            {
                result.IsSuccessful = true;
                result.OutputRes = Folders;
            }
            else
            {
                result.IsSuccessful = false;
                result.Message = "No directory found";
            }

            return result;
        }

        [HttpPut]
        public async Task<ResultContract> UpdateAsync(UpdateFolderDom input)
        {
            ResultContract result = new();
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

                    result.OutputRes = existingFolder;
                    result.Message = "Folder updated successfully";
                    result.IsSuccessful = true;

                    await _context.SaveChangesAsync();

                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = "Folder doesn't exist";
                }
            }
            else
            {
                result.IsSuccessful = false;
                result.Message = "Your folder path isn't valid.";
            }

            return result;
        }

        [HttpPost]
        public async Task<ResultContract> AddAsync(AddFolderDom input)
        {
            ResultContract result = new();
            string Path = input.Path.ToLower();

            if (Regex.IsMatch(Path, @"^[a-zA-Z]+$"))
            {

                bool exists = Directory.Exists(PathToFullPath(Path)) || _context.Folders.Any(folder => folder.Path == Path); ;

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

                    Directory.CreateDirectory(PathToFullPath(Path));
                    System.IO.File.Create(System.IO.Path.Combine(PathToFullPath(Path), ".gitkeep"));

                    result.OutputRes = AddedFolder.Entity;
                    result.Message = "Folder created successfully";
                    result.IsSuccessful = true;
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = "Folder already exists";
                }
            }
            else
            {
                result.IsSuccessful = false;
                result.Message = "Your folder path isn't valid.";
            }

            return result;
        }

        [HttpDelete]
        public async Task<ResultContract> DeleteAsync(long Id)
        {
            ResultContract result = new();

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
                
                Directory.Delete(PathToFullPath(entityToDelete.Path), true);

                _context.Folders.Remove(entityToDelete);

                await _context.SaveChangesAsync();

                result.IsSuccessful = true;
                result.Message = "Folder deleted successfully";
            }
            else
            {
                result.IsSuccessful = false;
                result.Message = "Folder not found";
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