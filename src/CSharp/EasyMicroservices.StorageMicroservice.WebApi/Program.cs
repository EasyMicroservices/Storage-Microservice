using EasyMicroservices.Cores.AspEntityFrameworkCoreApi;
using EasyMicroservices.Cores.Relational.EntityFrameworkCore.Intrerfaces;
using EasyMicroservices.FileManager.Interfaces;
using EasyMicroservices.FileManager.Providers.DirectoryProviders;
using EasyMicroservices.FileManager.Providers.FileProviders;
using EasyMicroservices.FileManager.Providers.PathProviders;
using EasyMicroservices.StorageMicroservice.Database.Contexts;

namespace EasyMicroservices.StorageMicroservice.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = CreateBuilder(args);
            var build = await app.Build<StorageContext>(true);
            build.MapControllers();
            await build.RunAsync();
        }

        static WebApplicationBuilder CreateBuilder(string[] args)
        {
            var app = StartUpExtensions.Create<StorageContext>(args);
            app.Services.Builder<StorageContext>().UseDefaultSwaggerOptions();
            app.Services.AddTransient(serviceProvider => new StorageContext(serviceProvider.GetService<IEntityFrameworkCoreDatabaseBuilder>()));
            app.Services.AddTransient<IEntityFrameworkCoreDatabaseBuilder, DatabaseBuilder>();

            string webRootPath = Directory.GetCurrentDirectory();
            app.Services.AddScoped<IPathProvider>(serviceProvider => new SystemPathProvider());
            app.Services.AddScoped<IDirectoryManagerProvider>(serviceProvider => new DiskDirectoryProvider(webRootPath));
            app.Services.AddTransient<IFileManagerProvider>(serviceProvider => new DiskFileProvider(new DiskDirectoryProvider(webRootPath)));
            StartUpExtensions.AddWhiteLabel("Storage", "RootAddresses:WhiteLabel");
            return app;
        }
    }
}