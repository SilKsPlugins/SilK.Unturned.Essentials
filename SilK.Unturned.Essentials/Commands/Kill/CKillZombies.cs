extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Games.Abstractions.Entities;
using OpenMod.Unturned.Zombies;
using SilK.Unturned.Essentials.Localization;
using System;
using System.Linq;

namespace SilK.Unturned.Essentials.Commands.Kill
{
    [UsedImplicitly]
    [Command("killzombies", Priority = Priority.Normal)]
    [CommandDescription("Kill all zombies.")]
    [LocalizationSection("Kill", "KillZombies")]
    public class CKillZombies : EssentialsCommand
    {
        private readonly IEntityDirectory _entityDirectory;

        public CKillZombies(IServiceProvider serviceProvider,
            IEntityDirectory entityDirectory) : base(serviceProvider)
        {
            _entityDirectory = entityDirectory;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync()
        {
            await UniTask.SwitchToMainThread();

            var entities = await _entityDirectory.GetEntitiesAsync();

            var zombies = entities.OfType<UnturnedZombie>().ToList();

            foreach (var zombie in zombies)
            {
                await zombie.KillAsync();
            }

            await PrintLocalizedAsync("Success", new { Amount = zombies.Count });
        }
    }
}