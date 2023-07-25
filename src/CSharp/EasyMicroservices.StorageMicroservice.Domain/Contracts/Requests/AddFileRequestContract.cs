using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public class AddFileRequestContract
    {
        [Required]
        public IFormFile File { get; set; }
        public long? FolderId { get; set; }
        public string UniqueIdentity { get; set; }
    }
}
