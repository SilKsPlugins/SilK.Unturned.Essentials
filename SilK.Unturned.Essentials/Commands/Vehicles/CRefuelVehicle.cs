extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using SilK.Unturned.Essentials.Helpers;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Vehicles
{
    [UsedImplicitly]
    [Command("refuelvehicle", Priority = Priority.Normal)]
    [CommandAlias("refuel")]
    [CommandSyntax("[all]")]
    [CommandDescription("Refuels the vehicle you are in or are looking at.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Vehicles", "RefuelVehicle")]
    public class CRefuelVehicle : EssentialsCommand
    {
        public CRefuelVehicle(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync()
        {
            await UniTask.SwitchToMainThread();
            
            var vehicle = UnturnedPlayer.CurrentVehicle ?? RaycastHelper.GetVehicleFromLook(UnturnedPlayer);

            if (vehicle == null)
            {
                throw GetLocalizedFriendlyException("NoVehicle");
            }

            vehicle.Vehicle.askFillFuel(ushort.MaxValue);

            await PrintLocalizedAsync("Success", new { Vehicle = vehicle });
        }
    }
}