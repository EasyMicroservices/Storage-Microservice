using EasyMicroservices.Cores.Contracts.Requests;

namespace EasyMicroservices.StorageMicroservice.Contracts.Requests
{
    public class GetByKeyRequestContract : GetByUniqueIdentityRequestContract
    {
        public string Key { get; set; }
    }
}
