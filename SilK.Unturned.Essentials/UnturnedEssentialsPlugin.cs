extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
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

            await _pluginComponentManager.LoadAsync();
        }

        protected override async UniTask OnUnloadAsync()
        {
            await _pluginComponentManager.UnloadAsync();
        }
    }
}
