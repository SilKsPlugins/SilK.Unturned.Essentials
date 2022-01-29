extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Games.Abstractions.Entities;
using OpenMod.Unturned.Animals;
using SilK.Unturned.Essentials.Localization;
using System;
using System.Linq;

namespace SilK.Unturned.Essentials.Commands.Kill
{
    [UsedImplicitly]
    [Command("killanimals", Priority = Priority.Normal)]
    [CommandDescription("Kill all animals.")]
    [LocalizationSection("Kill", "KillAnimals")]
    public class CKillAnimals : EssentialsCommand
    {
        private readonly IEntityDirectory _entityDirectory;

        public CKillAnimals(IServiceProvider serviceProvider,
            IEntityDirectory entityDirectory) : base(serviceProvider)
        {
            _entityDirectory = entityDirectory;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync()
        {
            await UniTask.SwitchToMainThread();

            var entities = await _entityDirectory.GetEntitiesAsync();

            var animals = entities.OfType<UnturnedAnimal>().ToList();

            foreach (var animal in animals)
            {
                await animal.KillAsync();
            }

            await PrintLocalizedAsync("Success", new { Amount = animals.Count });
        }
    }
}