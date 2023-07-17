using System.Threading.Tasks;
using EasyMicroservices.Mapper.CompileTimeMapper.Interfaces;
using EasyMicroservices.Mapper.Interfaces;
using System.Linq;

namespace CompileTimeMapper
{
    public class FolderEntity_FolderContract_Mapper : IMapper
    {
        readonly IMapperProvider _mapper;
        public FolderEntity_FolderContract_Mapper(IMapperProvider mapper)
        {
            _mapper = mapper;
        }

        public global::EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity Map(global::EasyMicroservices.StorageMicroservice.Contracts.FolderContract fromObject, string uniqueRecordId, string language, object[] parameters)
        {
            if (fromObject == default)
                return default;
            var mapped = new global::EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity()
            {
                CreationDateTime = fromObject.CreationDateTime,
                Id = fromObject.Id,
                Name = fromObject.Name,
                Path = fromObject.Path,
            };
            return mapped;
        }

        public global::EasyMicroservices.StorageMicroservice.Contracts.FolderContract Map(global::EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity fromObject, string uniqueRecordId, string language, object[] parameters)
        {
            if (fromObject == default)
                return default;
            var mapped = new global::EasyMicroservices.StorageMicroservice.Contracts.FolderContract()
            {
                CreationDateTime = fromObject.CreationDateTime,
                Id = fromObject.Id,
                Name = fromObject.Name,
                Path = fromObject.Path,
            };
            return mapped;
        }

        public async Task<global::EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity> MapAsync(global::EasyMicroservices.StorageMicroservice.Contracts.FolderContract fromObject, string uniqueRecordId, string language, object[] parameters)
        {
            if (fromObject == default)
                return default;
            var mapped = new global::EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity()
            {
                CreationDateTime = fromObject.CreationDateTime,
                Id = fromObject.Id,
                Name = fromObject.Name,
                Path = fromObject.Path,
            };
            return mapped;
        }

        public async Task<global::EasyMicroservices.StorageMicroservice.Contracts.FolderContract> MapAsync(global::EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity fromObject, string uniqueRecordId, string language, object[] parameters)
        {
            if (fromObject == default)
                return default;
            var mapped = new global::EasyMicroservices.StorageMicroservice.Contracts.FolderContract()
            {
                CreationDateTime = fromObject.CreationDateTime,
                Id = fromObject.Id,
                Name = fromObject.Name,
                Path = fromObject.Path,
            };
            return mapped;
        }
        public object MapObject(object fromObject, string uniqueRecordId, string language, object[] parameters)
        {
            if (fromObject == default)
                return default;
            if (fromObject.GetType() == typeof(EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity))
                return Map((EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity)fromObject, uniqueRecordId, language, parameters);
            return Map((EasyMicroservices.StorageMicroservice.Contracts.FolderContract)fromObject, uniqueRecordId, language, parameters);
        }
        public async Task<object> MapObjectAsync(object fromObject, string uniqueRecordId, string language, object[] parameters)
        {
            if (fromObject == default)
                return default;
            if (fromObject.GetType() == typeof(EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity))
                return await MapAsync((EasyMicroservices.StorageMicroservice.Database.Entities.FolderEntity)fromObject, uniqueRecordId, language, parameters);
            return await MapAsync((EasyMicroservices.StorageMicroservice.Contracts.FolderContract)fromObject, uniqueRecordId, language, parameters);
        }
    }
}