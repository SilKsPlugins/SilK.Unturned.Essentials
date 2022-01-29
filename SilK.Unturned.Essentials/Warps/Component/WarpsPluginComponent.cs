extern alias JetBrainsAnnotations;
using JetBrainsAnnotations::JetBrains.Annotations;
using SilK.Unturned.Essentials.API.Warps;
using SilK.Unturned.Essentials.Components;
using SilK.Unturned.Essentials.Warps.Source;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Warps.Component
{
    [UsedImplicitly]
    public class WarpsPluginComponent : IPluginComponent
    {
        private readonly EssentialsDataStoreWarpLocationSource _warpLocationSource;
        private readonly IWarpLocationDirectory _warpLocationDirectory;

        public WarpsPluginComponent(EssentialsDataStoreWarpLocationSource warpLocationSource,
            IWarpLocationDirectory warpLocationDirectory)
        {
            _warpLocationSource = warpLocationSource;
            _warpLocationDirectory = warpLocationDirectory;
        }

        public string Name => "Warps";

        public async Task LoadAsync()
        {
            await _warpLocationSource.LoadAsync();

            _warpLocationDirectory.AddSource(_warpLocationSource);
        }

        public Task UnloadAsync()
        {
            _warpLocationDirectory.RemoveSource(_warpLocationSource);

            return Task.CompletedTask;
        }
    }
}
