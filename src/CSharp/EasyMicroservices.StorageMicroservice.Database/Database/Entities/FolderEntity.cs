using EasyMicroservices.Cores.Interfaces;
using EasyMicroservices.StorageMicroservice.Database.Schemas;
using System.Collections.Generic;

namespace EasyMicroservices.StorageMicroservice.Database.Entities
{
    public class FolderEntity : FolderSchema, IIdSchema<long>
    {
        public long Id { get; set; }

        public ICollection<FileEntity> Files { get; set; }
    }
}
