using EasyMicroservices.Cores.Interfaces;
using System;

namespace EasyMicroservices.StorageMicroservice.Database.Schemas
{
    public class FileSchema : IUniqueIdentitySchema, IDateTimeSchema, ISoftDeleteSchema
    {
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Password { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public string UniqueIdentity { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
