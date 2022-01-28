extern alias JetBrainsAnnotations;
using JetBrainsAnnotations::JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
using OpenMod.Core.Commands;
using SilK.Unturned.Essentials.Commands.Items;
using SilK.Unturned.Essentials.Commands.Vehicles;

namespace SilK.Unturned.Essentials
{
    [UsedImplicitly]
    public class ServiceConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(IOpenModServiceConfigurationContext openModStartupContext,
            IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<CommandParameterResolverOptions>(options =>
            {
                options.AddCommandParameterResolveProvider<UnturnedItemAssetCommandParameterResolveProvider>();
                options.AddCommandParameterResolveProvider<UnturnedVehicleAssetCommandParameterResolveProvider>();
            });
        }
    }
}
