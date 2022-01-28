extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Items
{
    [UsedImplicitly]
    [Command("respawnitems", Priority = Priority.Normal)]
    [CommandAlias("dropitem")]
    [CommandSyntax("<x> <y> <z> <item> [amount]")]
    [CommandDescription("Respawns all items.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Items", "RespawnItems")]
    public class CRespawnItems : EssentialsCommand
    {
        public CRespawnItems(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync()
        {
            await UniTask.SwitchToMainThread();

            foreach (var itemSpawns in LevelItems.spawns)
            {
                foreach (var itemSpawn in itemSpawns)
                {
                    var itemId = LevelItems.getItem(itemSpawn);

                    if (itemId == 0)
                    {
                        continue;
                    }

                    var item = new Item(itemId, EItemOrigin.WORLD);

                    ItemManager.dropItem(item, itemSpawn.point, false, false, false);
                }
            }

            await PrintLocalizedAsync("Success");
        }
    }
}