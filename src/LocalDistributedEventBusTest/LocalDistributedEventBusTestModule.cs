using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LocalDistributedEventBusTest
{

    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpEventBusModule)
    )]
    public class LocalDistributedEventBusTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHostedService<LocalDistributedEventBusTestHostedService>();
        }
    }
}
