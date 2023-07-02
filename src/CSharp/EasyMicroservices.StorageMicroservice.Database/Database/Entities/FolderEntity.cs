using EasyMicroservices.StorageMicroservice.Database.Schemas;
using System.Collections.Generic;

namespace EasyMicroservices.StorageMicroservice.Database.Entities
{
    public class FolderEntity : FolderSchema
    {
        public long Id { get; set; }

        public ICollection<FileEntity> Files { get; set; }
    }
}
