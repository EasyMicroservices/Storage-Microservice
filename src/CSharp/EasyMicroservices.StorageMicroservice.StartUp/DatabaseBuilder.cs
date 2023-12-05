using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EasyMicroservices.StorageMicroservice
{
    public class DatabaseBuilder : IEntityFrameworkCoreDatabaseBuilder
    {
        public DatabaseBuilder(IConfiguration configuration)
        {
            _config = configuration;
        }

        readonly IConfiguration _config;

        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("Storage");
            optionsBuilder.UseSqlServer(_config.GetConnectionString("local"));
        }
    }
}
