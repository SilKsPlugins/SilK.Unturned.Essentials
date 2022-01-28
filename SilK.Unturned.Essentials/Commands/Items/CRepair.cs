extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Items;
using OpenMod.Unturned.Users;
using SilK.Unturned.Essentials.Localization;
using System;
using System.Linq;

namespace SilK.Unturned.Essentials.Commands.Items
{
    [UsedImplicitly]
    [Command("repair", Priority = Priority.Normal)]
    [CommandAlias("repairall")]
    [CommandAlias("fix")]
    [CommandAlias("fixall")]
    [CommandDescription("Repairs all items in your inventory.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Items", "Repair")]
    public class CRepair : EssentialsCommand
    {
        public CRepair(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync()
        {
            await UniTask.SwitchToMainThread();

            var amount = 0;

            foreach (var item in UnturnedPlayer.Inventory.Pages.SelectMany(x => x.Items).OfType<UnturnedInventoryItem>())
            {
                var maxQuality = item.Item.Asset.MaxQuality;

                if (!maxQuality.HasValue)
                {
                    continue;
                }

                await item.Item.SetItemQualityAsync(maxQuality.Value);

                amount++;
            }

            await PrintLocalizedAsync("Success", new { Amount = amount });
        }
    }
}