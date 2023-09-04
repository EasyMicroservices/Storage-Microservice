using System;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public class FileContract
    {
        public long Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public string Password { get; set; }
        public long FolderId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string DownloadLink { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
