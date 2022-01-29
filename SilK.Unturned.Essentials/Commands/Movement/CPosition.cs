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

namespace SilK.Unturned.Essentials.Commands.Movement
{
    [UsedImplicitly]
    [Command("position", Priority = Priority.Normal)]
    [CommandAlias("pos")]
    [CommandAlias("coords")]
    [CommandAlias("coord")]
    [CommandAlias("xyz")]
    [CommandSyntax("[player]")]
    [CommandDescription("Ascend the specified distance (default 10 metres).")]
    [LocalizationSection("Movement", "Ascend")]
    [RegisterCommandPermission(OtherPermission, DefaultGrant = PermissionGrantResult.Deny,
        Description = "Allows getting the position of another player.")]
    public class CPosition : EssentialsCommand
    {
        private const string OtherPermission = "other";

        public CPosition(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(UnturnedPlayer? player = null)
        {
            if (player != null && await CheckPermissionAsync(OtherPermission) != PermissionGrantResult.Grant)
            {
                throw new NotEnoughPermissionException(Context, OtherPermission);
            }

            await UniTask.SwitchToMainThread();

            var pos = player?.Transform.Position ?? UnturnedPlayer.Transform.Position;

            if (player == null)
            {
                await PrintLocalizedAsync("Success", new { pos.X, pos.Y, pos.Z });
            }
            else
            {
                await PrintLocalizedAsync("SuccessOther", new { pos.X, pos.Y, pos.Z, Player = player });
            }
        }
    }
}