extern alias JetBrainsAnnotations;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;

[assembly: PluginMetadata("SilK.Unturned.Essentials", DisplayName = "SilK's Unturned Essentials", Author = "SilK")]

namespace SilK.Unturned.Essentials
{
    [UsedImplicitly]
    public class UnturnedEssentialsPlugin : OpenModUnturnedPlugin
    {
        public UnturnedEssentialsPlugin(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
