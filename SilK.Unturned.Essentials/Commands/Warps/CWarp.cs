extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Core.Cooldowns;
using OpenMod.Unturned.Users;
using SilK.Unturned.Essentials.API.Warps;
using SilK.Unturned.Essentials.Helpers;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Warps
{
    [UsedImplicitly]
    [Command("warp", Priority = Priority.Normal)]
    [CommandSyntax("<warp name>")]
    [CommandDescription("Teleport to a warp.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Warps", "Warp")]
    public class CWarp : EssentialsCommand
    {
        private readonly ICommandCooldownStore _cooldownStore;
        
        public CWarp(IServiceProvider serviceProvider,
            ICommandCooldownStore cooldownStore) : base(serviceProvider)
        {
            _cooldownStore = cooldownStore;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(IWarpLocation warp)
        {
            var cooldownId = GetWarpCooldownId(warp);

            if (warp.Cooldown > TimeSpan.Zero)
            {
                var timeLeft = await _cooldownStore.GetTimeLeft(Context.Actor, cooldownId, warp.Cooldown);

                if (timeLeft > TimeSpan.Zero)
                {
                    throw GetLocalizedFriendlyException("Cooldown", new { Warp = warp, TimeLeft = timeLeft });
                }
            }

            if (await UnturnedPlayer.SetPositionAsync(warp.Position, warp.Rotation))
            {
                await _cooldownStore.RecordExecutionAsync(Context.Actor, cooldownId, DateTime.Now);

                await PrintLocalizedAsync("Success", new { Warp = warp });
            }
            else
            {
                await PrintLocalizedAsync("Failure", new { Warp = warp });
            }
        }

        private string GetWarpCooldownId(IWarpLocation warp)
        {
            return $"{Plugin.OpenModComponentId}:warps.{warp.Name}";
        }
    }
}