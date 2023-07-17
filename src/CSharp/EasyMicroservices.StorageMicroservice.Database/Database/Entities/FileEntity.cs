using EasyMicroservices.StorageMicroservice.Database.Schemas;

namespace EasyMicroservices.StorageMicroservice.Database.Entities
{
    public class FileEntity : FileSchema
    {
        public long Id { get; set; }
        public string Guid { get; set; }

        public string Password { get; set; }
        public long FolderId { get; set; }
        public FolderEntity Folder { get; set; }
    }
}
