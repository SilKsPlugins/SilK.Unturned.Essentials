extern alias JetBrainsAnnotations;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.Unturned.Plugins;
using System;

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
