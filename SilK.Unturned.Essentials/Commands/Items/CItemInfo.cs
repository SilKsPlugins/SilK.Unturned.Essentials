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

namespace SilK.Unturned.Essentials.Commands.Items
{
    [UsedImplicitly]
    [Command("iteminfo", Priority = Priority.Normal)]
    [CommandDescription("Shows info of an item.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Items", "ItemInfo")]
    public class CItemInfo : EssentialsCommand
    {
        public CItemInfo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(UnturnedItemAsset item)
        {
            await PrintLocalizedAsync("Success", new { ItemAsset = item });
        }
    }
}
