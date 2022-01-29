extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using SilK.Unturned.Essentials.Components;
using System;

[assembly: PluginMetadata("SilK.Unturned.Essentials", DisplayName = "SilK's Unturned Essentials", Author = "SilK")]

namespace SilK.Unturned.Essentials
{
    [UsedImplicitly]
    public class UnturnedEssentialsPlugin : OpenModUnturnedPlugin
    {
        private readonly PluginComponentManager _pluginComponentManager;

        public UnturnedEssentialsPlugin(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _pluginComponentManager = new PluginComponentManager(this);
        }

        protected override async UniTask OnLoadAsync()
        {
            Logger.LogInformation("");
            Logger.LogInformation("--------------------------------------------------------------------");
            Logger.LogInformation("> Loading SilK's Unturned Essentials");
            Logger.LogInformation("--------------------------------------------------------------------");
            Logger.LogInformation("For more support, idea/feature requests, or to check out my plugins:");
            Logger.LogInformation("Join my Discord server - https://discord.gg/c8Qr7drK4m");
            Logger.LogInformation("--------------------------------------------------------------------");
            Logger.LogInformation("");

            await _pluginComponentManager.LoadAsync();
        }

        protected override async UniTask OnUnloadAsync()
        {
            await _pluginComponentManager.UnloadAsync();
        }
    }
}
