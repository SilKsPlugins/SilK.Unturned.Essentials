extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Games.Abstractions.Items;
using OpenMod.Unturned.Items;
using OpenMod.Unturned.Users;
using SilK.Unturned.Essentials.Localization;
using System;
using System.Numerics;

namespace SilK.Unturned.Essentials.Commands.Items
{
    [UsedImplicitly]
    [Command("spawnitem", Priority = Priority.Normal)]
    [CommandAlias("dropitem")]
    [CommandSyntax("<x> <y> <z> <item> [amount]")]
    [CommandDescription("Spawns items at the specified position.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Items", "SpawnItem")]
    public class CSpawnItem : EssentialsCommand
    {
        private readonly IItemSpawner _itemSpawner;

        public CSpawnItem(IServiceProvider serviceProvider,
            IItemSpawner itemSpawner) : base(serviceProvider)
        {
            _itemSpawner = itemSpawner;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(float x, float y, float z, UnturnedItemAsset item, int amount = 1)
        {
            var success = true;

            var position = new Vector3(x, y, z);

            await UniTask.SwitchToMainThread();

            for (var i = 0; i < amount; i++)
            {
                if (await _itemSpawner.SpawnItemAsync(position, item) == null)
                {
                    success = false;
                }
            }

            if (!success)
            {
                throw GetLocalizedFriendlyException("Failure",
                    new { X = x, Y = y, Z = z, ItemAsset = item, Amount = amount });
            }

            await PrintLocalizedAsync("Success",
                new { X = x, Y = y, Z = z, ItemAsset = item, Amount = amount });
        }
    }
}
