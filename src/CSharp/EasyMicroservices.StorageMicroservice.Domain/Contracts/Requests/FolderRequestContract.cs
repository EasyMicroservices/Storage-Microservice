using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public class AddFolderContract
    {
        public AddFolderContract()
        {
            Name = string.Empty;
            Path = string.Empty;
        }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Folder name must be between 6 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(63, MinimumLength = 5, ErrorMessage = "Folder path must be between 6 and 100 characters.")]
        public string Path { get; set; }
    }

    public class UpdateFolderContract
    {
        public UpdateFolderContract()
        {
            Name = string.Empty;
            Path = string.Empty;
        }
        public long Id { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Folder name must be between 6 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(63, MinimumLength = 5, ErrorMessage = "Folder path must be between 6 and 100 characters.")]
        public string Path { get; set; }
    }
}
