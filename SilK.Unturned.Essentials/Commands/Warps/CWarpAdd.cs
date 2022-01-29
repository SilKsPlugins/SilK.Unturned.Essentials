extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using SilK.Unturned.Essentials.Localization;
using SilK.Unturned.Essentials.Warps.Source;
using System;

namespace SilK.Unturned.Essentials.Commands.Warps
{
    [UsedImplicitly]
    [CommandParent(typeof(CWarp))]
    [Command("add", Priority = Priority.Normal)]
    [CommandSyntax("<warp name> [cooldown]")]
    [CommandDescription("Add a new warp location.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Warps", "WarpAdd")]
    public class CWarpAdd : EssentialsCommand
    {
        private readonly EssentialsDataStoreWarpLocationSource _warpLocationSource;

        public CWarpAdd(IServiceProvider serviceProvider,
            EssentialsDataStoreWarpLocationSource warpLocationSource) : base(serviceProvider)
        {
            _warpLocationSource = warpLocationSource;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(string warpName, TimeSpan cooldown = default)
        {
            warpName = warpName.ToLower();

            var position = UnturnedPlayer.Transform.Position;
            var rotation = UnturnedPlayer.Transform.Rotation;

            var added = await _warpLocationSource.AddOrUpdateAsync(warpName, data =>
            {
                data.Name = warpName;
                data.Position = position;
                data.Rotation = rotation;
                data.Cooldown = cooldown;
            });

            await PrintLocalizedAsync(added ? "SuccessAdded" : "SuccessUpdated",
                new { WarpName = warpName, Cooldown = cooldown });
        }
    }
}