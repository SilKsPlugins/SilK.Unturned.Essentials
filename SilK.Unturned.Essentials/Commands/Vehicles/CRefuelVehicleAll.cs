extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Games.Abstractions.Vehicles;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Vehicles;
using SilK.Unturned.Essentials.Localization;
using System;
using System.Linq;

namespace SilK.Unturned.Essentials.Commands.Vehicles
{
    [UsedImplicitly]
    [CommandParent(typeof(CRefuelVehicle))]
    [Command("all", Priority = Priority.Normal)]
    [CommandDescription("Refuels all vehicles.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Vehicles", "RefuelVehicleAll")]
    public class CRefuelVehicleAll : EssentialsCommand
    {
        private readonly IVehicleDirectory _vehicleDirectory;

        public CRefuelVehicleAll(IServiceProvider serviceProvider,
            IVehicleDirectory vehicleDirectory) : base(serviceProvider)
        {
            _vehicleDirectory = vehicleDirectory;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync()
        {
            await UniTask.SwitchToMainThread();

            var vehicles = await _vehicleDirectory.GetVehiclesAsync();

            await UniTask.SwitchToMainThread();

            foreach (var vehicle in vehicles.OfType<UnturnedVehicle>())
            {
                vehicle.Vehicle.askFillFuel(ushort.MaxValue);
            }

            await PrintLocalizedAsync("Success", new { Amount = vehicles.Count });
        }
    }
}