extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Kill
{
    [UsedImplicitly]
    [Command("killall", Priority = Priority.Normal)]
    [CommandDescription("Kill all players.")]
    [LocalizationSection("Kill", "KillAll")]
    public class CKillAll : EssentialsCommand
    {
        private readonly IUnturnedUserDirectory _unturnedUserDirectory;

        public CKillAll(IServiceProvider serviceProvider,
            IUnturnedUserDirectory unturnedUserDirectory) : base(serviceProvider)
        {
            _unturnedUserDirectory = unturnedUserDirectory;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync()
        {
            await UniTask.SwitchToMainThread();

            var users = _unturnedUserDirectory.GetOnlineUsers();

            foreach (var user in users)
            {
                await user.Player.KillAsync();
            }

            await PrintLocalizedAsync("Success", new {Amount = users.Count});
        }
    }
}