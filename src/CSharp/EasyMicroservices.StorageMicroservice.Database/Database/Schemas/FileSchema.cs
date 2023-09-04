using EasyMicroservices.Cores.Database.Schemas;

namespace EasyMicroservices.StorageMicroservice.Database.Schemas
{
    public class FileSchema : FullAbilitySchema
    {
        public string Name { get; set; }
        /// <summary>
        /// every files can have key to separate them in groups
        /// because of unique identity cannot separate them
        /// </summary>
        public string Key { get; set; }
        public string Extension { get; set; }
        public string Password { get; set; }
        public long Length { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
    }
}
