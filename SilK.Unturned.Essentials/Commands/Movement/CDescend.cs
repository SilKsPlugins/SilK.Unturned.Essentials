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
    [Command("descend", Priority = Priority.Normal)]
    [CommandAlias("desc")]
    [CommandSyntax("[distance]")]
    [CommandDescription("Descend the specified distance (default 10 metres).")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Movement", "Descend")]
    public class CDescend : EssentialsCommand
    {
        public CDescend(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(float distance = 10)
        {
            await UniTask.SwitchToMainThread();

            var position = UnturnedPlayer.Transform.Position;

            position.Y -= distance;

            if (!await UnturnedPlayer.SetPositionAsync(position))
            {
                throw GetLocalizedFriendlyException("Failure", new { Distance = distance });
            }

            await PrintLocalizedAsync("Success", new { Distance = distance });
        }
    }
}