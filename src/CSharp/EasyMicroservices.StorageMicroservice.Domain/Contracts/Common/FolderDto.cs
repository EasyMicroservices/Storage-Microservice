using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public class GetAllFoldersDto
    {
        public GetAllFoldersDto()
        {
            Name = string.Empty;
            Path = string.Empty;
        }
        public long Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
