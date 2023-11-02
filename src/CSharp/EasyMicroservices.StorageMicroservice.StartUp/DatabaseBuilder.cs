using EasyMicroservices.StorageMicroservice.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;

namespace EasyMicroservices.StorageMicroservice
{
    public class DatabaseBuilder : IEntityFrameworkCoreDatabaseBuilder
    {
        IConfiguration _configuration;
        public DatabaseBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Storage");
            //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("local"));
        }
    }
}
