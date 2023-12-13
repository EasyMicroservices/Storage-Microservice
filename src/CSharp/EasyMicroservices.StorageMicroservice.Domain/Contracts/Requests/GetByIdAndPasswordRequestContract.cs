namespace EasyMicroservices.StorageMicroservice.Contracts.Requests;
public class GetByIdAndPasswordRequestContract
{
    public long Id { get; set; }
    public string Password { get; set; }
}
