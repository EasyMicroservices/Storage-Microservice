using EasyMicroservices.StorageMicroservice.Database.Contexts;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;
using EasyMicroservices.ContentsMicroservice.Helpers;
using EasyMicroservices.Cores.AspEntityFrameworkCoreApi.Interfaces;

namespace EasyMicroservices.StorageMicroservice.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = CreateBuilder(args);
            var build = await app.Build<StorageContext>();
            build.MapControllers();
            build.Run();
        }

        static WebApplicationBuilder CreateBuilder(string[] args)
        {
            var app = StartUpExtensions.Create<StorageContext>(args);
            app.Services.Builder<StorageContext>();
            app.Services.AddTransient((serviceProvider) => new UnitOfWork(serviceProvider));
            app.Services.AddTransient(serviceProvider => new StorageContext(serviceProvider.GetService<IEntityFrameworkCoreDatabaseBuilder>()));
            app.Services.AddTransient<IEntityFrameworkCoreDatabaseBuilder, DatabaseBuilder>();
            app.Services.AddScoped<IAppUnitOfWork>((serviceProvider) => new AppUnitOfWork(serviceProvider));

            StartUpExtensions.AddWhiteLabel("Content", "RootAddresses:WhiteLabel");
            return app;
        }

        public static async Task Run(string[] args, Action<IServiceCollection> use)
        {
            var app = CreateBuilder(args);
            use?.Invoke(app.Services);
            var build = await app.Build<StorageContext>();
            build.MapControllers();
            build.Run();
        }
    }
}