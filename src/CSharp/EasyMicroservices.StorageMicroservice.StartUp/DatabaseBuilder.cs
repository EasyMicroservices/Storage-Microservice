using EasyMicroservices.StorageMicroservice.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;

namespace EasyMicroservices.StorageMicroservice
{
    public class DatabaseBuilder : IDatabaseBuilder
    {
        readonly IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("Storage");
            optionsBuilder.UseSqlServer(config.GetConnectionString("local"));
        }
    }
}
