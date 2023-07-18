using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public class AddFileResultContract
    {
        public System.Guid Guid { get; set; }

        public DateTime CreationDateTime { get; set; }

        public string Name { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }

        public string Password { get; set; }
        public long FolderId { get; set; }
        public string DownloadLink { get; set; }
    }
}
