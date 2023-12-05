using EasyMicroservices.ContentsMicroservice.Helpers;
using EasyMicroservices.FileManager.Interfaces;
using System;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Logics
{
    public static class FileLogic
    {
        public static async Task<string> NameToFullPath(string fileName, IAppUnitOfWork _unitOfWork)
        {
            string webRootPath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = _unitOfWork.GetDirectoryManagerProvider().PathProvider.Combine(webRootPath, "StorageFiles");
            string filePath = _unitOfWork.GetDirectoryManagerProvider().PathProvider.Combine(directoryPath, fileName);
            if (!await _unitOfWork.GetDirectoryManagerProvider().IsExistDirectoryAsync(directoryPath))
            {
                await _unitOfWork.GetDirectoryManagerProvider().CreateDirectoryAsync(directoryPath);
            }
            return filePath;
        }
    }
}
