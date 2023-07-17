using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public class Constants
    {
        public static string[] AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".txt" };
        public static string RootAddress = "storage";
    }
}
