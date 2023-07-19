using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public class Constants
    {
        public static string[] AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png"};
        public static string[] ImageExtensions = { ".bmp", ".gif", ".jpeg", ".jpg", ".png", ".tif", ".tiff" };
        public static string RootAddress = "storage";
    }
}
