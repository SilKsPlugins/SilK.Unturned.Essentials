extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Items;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Items
{
    [UsedImplicitly]
    [Command("repairhand", Priority = Priority.Normal)]
    [CommandAlias("fixhand")]
    [CommandDescription("Repairs the item in your hand.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Items", "RepairHand")]
    public class CRepairHand : EssentialsCommand
    {
        public CRepairHand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync()
        {
            await UniTask.SwitchToMainThread();

            var (item, asset, page, x, y) = GetEquippedItem();
            
            if (item == null || asset == null)
            {
                throw GetLocalizedFriendlyException("NoEquipped");
            }

            UnturnedPlayer.Inventory.Inventory.sendUpdateQuality(page, x, y, asset.qualityMax);

            var unturnedItemAsset = new UnturnedItemAsset(asset);

            await PrintLocalizedAsync("Success", new { ItemAsset = unturnedItemAsset });
        }

        private (ItemJar? Item, ItemAsset? ItemAsset, byte Page, byte X, byte Y) GetEquippedItem()
        {
            var nativePlayer = UnturnedPlayer.Player;

            var equipment = nativePlayer.equipment;

            var asset = equipment.asset;
            var page = equipment.equippedPage;
            var x = equipment.equipped_x;
            var y = equipment.equipped_y;

            var index = nativePlayer.inventory.getIndex(page, x, y);

            if (index == byte.MaxValue)
            {
                return (null, null, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }

            var item = nativePlayer.inventory.getItem(page, index);

            return (item, asset, page, x, y);
        }
    }
}