using EasyMicroservices.Cores.AspEntityFrameworkCoreApi;
using EasyMicroservices.FileManager.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Helpers
{
    public class AppUnitOfWork : UnitOfWork, IAppUnitOfWork
    {
        public AppUnitOfWork(IServiceProvider service) : base(service)
        {
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
