using EasyMicroservices.StorageMicroservice.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice
{
    public class DatabaseBuilder : IDatabaseBuilder
    {
        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("Storage database");
            optionsBuilder.UseSqlServer("Server=.;Database=Storage;User ID=test;Password=test1234;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
