using System;
using System.IO;
using System.Reflection;
using LocalDistributedEventBusTest.Books;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;

namespace LocalDistributedEventBusTest
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpTestBaseModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpBlobStoringFileSystemModule),
        typeof(AbpMemoryDbModule),
        typeof(AbpDddApplicationModule),
        typeof(LocalDistributedEventBusTestModule)
    )]
    public class LocalDistributedEventBusTestTestsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = GetFileBlobContainerBathPath();
                    });
                });
            });
            
            var connStr = Guid.NewGuid().ToString();

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connStr;
            });
            
            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.AutoEventSelectors.Add<Book>();
                options.EtoMappings.Add<Book, Book>();
            });

            context.Services.AddMemoryDbContext<TestMemoryDbContext>(options =>
            {
                options.AddDefaultRepositories();
                options.AddRepository<Book, BookRepository>();
            });
        }

        private static string GetFileBlobContainerBathPath()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                "my-files");
        }
    }
}
