using EasyMicroservices.Cores.Contracts.Requests;

namespace EasyMicroservices.StorageMicroservice.Contracts.Requests
{
    public class GetByKeyRequestContract : GetUniqueIdentityRequestContract
    {
        public string Key { get; set; }
    }
}
