extern alias JetBrainsAnnotations;
using Autofac;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Plugins;
using SilK.Unturned.Essentials.Warps.Source;

namespace SilK.Unturned.Essentials.IoC
{
    [UsedImplicitly]
    public class PluginContainerConfigurator : IPluginContainerConfigurator
    {
        public void ConfigureContainer(IPluginServiceConfigurationContext context)
        {
            context.ContainerBuilder.RegisterType<EssentialsDataStoreWarpLocationSource>()
                .SingleInstance();
        }
    }
}
