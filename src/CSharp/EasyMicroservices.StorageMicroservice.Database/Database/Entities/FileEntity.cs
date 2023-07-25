using EasyMicroservices.Cores.Interfaces;
using EasyMicroservices.StorageMicroservice.Database.Schemas;

namespace EasyMicroservices.StorageMicroservice.Database.Entities
{
    public class FileEntity : FileSchema, IIdSchema<long>
    {
        public long Id { get; set; }

        public long FolderId { get; set; }
        public FolderEntity Folder { get; set; }
    }
}
