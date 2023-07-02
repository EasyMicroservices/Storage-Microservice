using System;

namespace EasyMicroservices.StorageMicroservice.Database.Schemas
{
    public class FolderSchema
    {
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
