using EasyMicroservices.ContentsMicroservice.Helpers;
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
            var build = await app.BuildWithUseCors<StorageContext>(null, true);
            build.MapControllers();
            var filesPath =  build.Configuration.GetValue<string>("DiskFilesPath");
            if (filesPath != ".")
                WebRootPath = filesPath;
            await build.RunAsync();
        }
        static string WebRootPath = Directory.GetCurrentDirectory();

        static WebApplicationBuilder CreateBuilder(string[] args)
        {
            var app = StartUpExtensions.Create<StorageContext>(args);
            app.Services.Builder<StorageContext>("Storage")
                .UseDefaultSwaggerOptions();
            app.Services.AddTransient(serviceProvider => new StorageContext(serviceProvider.GetService<IEntityFrameworkCoreDatabaseBuilder>()));
            app.Services.AddTransient<IEntityFrameworkCoreDatabaseBuilder, DatabaseBuilder>();
            app.Services.AddTransient<IAppUnitOfWork>(serviceProvider => new AppUnitOfWork(serviceProvider));

            app.Services.AddScoped<IPathProvider>(serviceProvider => new SystemPathProvider());
            app.Services.AddScoped<IDirectoryManagerProvider>(serviceProvider => new DiskDirectoryProvider(WebRootPath));
            app.Services.AddTransient<IFileManagerProvider>(serviceProvider => new DiskFileProvider(new DiskDirectoryProvider(WebRootPath)));
            return app;
        }
    }
}