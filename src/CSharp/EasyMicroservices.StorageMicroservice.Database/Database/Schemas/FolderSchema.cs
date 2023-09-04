using EasyMicroservices.Cores.Database.Schemas;

namespace EasyMicroservices.StorageMicroservice.Database.Schemas
{
    public class FolderSchema : FullAbilitySchema
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
