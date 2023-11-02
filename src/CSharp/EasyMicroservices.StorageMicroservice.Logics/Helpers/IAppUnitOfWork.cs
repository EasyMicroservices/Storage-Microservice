using EasyMicroservices.Cores.AspEntityFrameworkCoreApi.Interfaces;
using EasyMicroservices.FileManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.ContentsMicroservice.Helpers
{
    public interface IAppUnitOfWork : IUnitOfWork
    {
        public IFileManagerProvider GetFileManagerProvider();
        public IDirectoryManagerProvider GetDirectoryManagerProvider();

    }
}
