using System;

namespace EasyMicroservices.StorageMicroservice.Database.Schemas
{
    public class FileSchema
    {
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }

        public string Name { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
    }
}
