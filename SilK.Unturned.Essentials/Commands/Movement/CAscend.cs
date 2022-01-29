extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Movement
{
    [UsedImplicitly]
    [Command("ascend", Priority = Priority.Normal)]
    [CommandAlias("asc")]
    [CommandSyntax("[distance]")]
    [CommandDescription("Ascend the specified distance (default 10 metres).")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Movement", "Ascend")]
    public class CAscend : EssentialsCommand
    {
        public CAscend(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(float distance = 10)
        {
            await UniTask.SwitchToMainThread();

            var position = UnturnedPlayer.Transform.Position;

            position.Y += distance;

            if (!await UnturnedPlayer.SetPositionAsync(position))
            {
                throw GetLocalizedFriendlyException("Failure", new { Distance = distance });
            }

            await PrintLocalizedAsync("Success", new { Distance = distance });
        }
    }
}