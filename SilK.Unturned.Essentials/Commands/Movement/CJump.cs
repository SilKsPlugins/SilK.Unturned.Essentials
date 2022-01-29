extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using SilK.Unturned.Essentials.Helpers;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Movement
{
    [UsedImplicitly]
    [Command("jump", Priority = Priority.Normal)]
    [CommandSyntax("[max distance]")]
    [CommandDescription("Teleport to where you're looking at.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Movement", "Jump")]
    public class CJump : EssentialsCommand
    {
        public CJump(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(float maxDistance = 1024)
        {
            await UniTask.SwitchToMainThread();

            var position = RaycastHelper.GetLookPosition(UnturnedPlayer, maxDistance) ??
                           throw GetLocalizedFriendlyException("NoPoint", new { MaxDistance = maxDistance });

            if (!await UnturnedPlayer.SetPositionAsync(position))
            {
                throw GetLocalizedFriendlyException("Failure", new { MaxDistance = maxDistance });
            }

            await PrintLocalizedAsync("Success", new { MaxDistance = maxDistance });
        }
    }
}