using EasyMicroservices.Cores.AspEntityFrameworkCoreApi;
using EasyMicroservices.FileManager.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EasyMicroservices.ContentsMicroservice.Helpers
{
    public class AppUnitOfWork : UnitOfWork, IAppUnitOfWork
    {
        IServiceProvider _service;
        public AppUnitOfWork(IServiceProvider service) : base(service)
        {
            _service = service;
        }

        public IDirectoryManagerProvider GetDirectoryManagerProvider()
        {
            return _service.GetService<IDirectoryManagerProvider>();
        }

        public IFileManagerProvider GetFileManagerProvider()
        {
            return _service.GetService<IFileManagerProvider>();
        }
    }
}
