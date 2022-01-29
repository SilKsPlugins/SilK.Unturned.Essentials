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
using System.ComponentModel.DataAnnotations;

namespace SilK.Unturned.Essentials.Commands.Items
{
    [UsedImplicitly]
    [Command("item", Priority = Priority.Normal)]
    [CommandAlias("i")]
    [CommandSyntax("<item> [amount]")]
    [CommandDescription("Spawns items.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Items", "Item")]
    public class CItem : EssentialsCommand
    {
        private readonly IItemSpawner _itemSpawner;

        public CItem(IServiceProvider serviceProvider,
            IItemSpawner itemSpawner) : base(serviceProvider)
        {
            _itemSpawner = itemSpawner;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(UnturnedItemAsset item, [Range(1, int.MaxValue)] int amount = 1)
        {
            var success = true;

            await UniTask.SwitchToMainThread();

            for (var i = 0; i < amount; i++)
            {
                if (await _itemSpawner.GiveItemAsync(UnturnedPlayer.Inventory, item) == null)
                {
                    success = false;
                }
            }

            if (!success)
            {
                throw GetLocalizedFriendlyException("Failure", new { ItemAsset = item, Amount = amount });
            }

            await PrintLocalizedAsync("Success", new { ItemAsset = item, Amount = amount });
        }
    }
}
