using EasyMicroservices.FileManager.Interfaces;
using System;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Logics
{
    public static class FileLogic
    {
        public static async Task<string> NameToFullPath(string fileName, IDirectoryManagerProvider directoryManagerProvider)
        {
            string webRootPath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = directoryManagerProvider.PathProvider.Combine(webRootPath, "StorageFiles");
            string filePath = directoryManagerProvider.PathProvider.Combine(directoryPath, fileName);
            if (!await directoryManagerProvider.IsExistDirectoryAsync(directoryPath))
            {
                await directoryManagerProvider.CreateDirectoryAsync(directoryPath);
            }
            return filePath;
        }
    }
}
