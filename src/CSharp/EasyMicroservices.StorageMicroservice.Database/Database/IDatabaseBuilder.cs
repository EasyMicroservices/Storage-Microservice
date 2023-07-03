using Microsoft.EntityFrameworkCore;

namespace EasyMicroservices.StorageMicroservice.Database
{
    public interface IDatabaseBuilder
    {
        void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
    }
}
