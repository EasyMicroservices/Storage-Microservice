using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public class AddFileContract
    {
        [Required]
        public IFormFile File { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "File password length must be between 5 and 100 characters.")]
        public string Password { get; set; }

        public long? FolderId { get; set; }
    }
}
