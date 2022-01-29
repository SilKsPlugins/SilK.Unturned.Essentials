extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Permissions;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Core.Permissions;
using OpenMod.Unturned.Players;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Kill
{
    [UsedImplicitly]
    [Command("kill", Priority = Priority.Normal)]
    [CommandSyntax("[player]")]
    [CommandDescription("Kill yourself or the specified player.")]
    [LocalizationSection("Kill", "Kill")]
    [RegisterCommandPermission(OtherPermission, DefaultGrant = PermissionGrantResult.Deny,
        Description = "Allows using /kill on another player.")]
    public class CKill : EssentialsCommand
    {
        private const string OtherPermission = "other";

        public CKill(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(UnturnedPlayer? player = null)
        {
            if (player != null && await CheckPermissionAsync(OtherPermission) != PermissionGrantResult.Grant)
            {
                throw new NotEnoughPermissionException(Context, OtherPermission);
            }

            var other = player == null;

            player ??= UnturnedPlayer;

            await player.KillAsync();

            await (other
                ? PrintLocalizedAsync("SuccessOther", new { Player = player })
                : PrintLocalizedAsync("Success"));
        }
    }
}