using EasyMicroservices.StorageMicroservice.Helpers;
using EasyMicroservices.StorageMicroservice.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice
{
    public class StartUp
    {
        public Task Run(IDependencyManager dependencyManager)
        {
            ApplicationManager.Instance.DependencyManager = dependencyManager;
            return Task.CompletedTask;
        }
    }
}
