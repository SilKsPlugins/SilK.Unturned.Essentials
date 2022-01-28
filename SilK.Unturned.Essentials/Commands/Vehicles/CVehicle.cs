extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Games.Abstractions.Vehicles;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Vehicles;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Vehicles
{
    [UsedImplicitly]
    [Command("vehicle", Priority = Priority.Normal)]
    [CommandAlias("v")]
    [CommandSyntax("<vehicle>")]
    [CommandDescription("Spawns a vehicle.")]
    [CommandActor(typeof(UnturnedUser))]
    [LocalizationSection("Vehicles", "Vehicle")]
    public class CVehicle : EssentialsCommand
    {
        private readonly IVehicleSpawner _vehicleSpawner;

        public CVehicle(IServiceProvider serviceProvider,
            IVehicleSpawner vehicleSpawner) : base(serviceProvider)
        {
            _vehicleSpawner = vehicleSpawner;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(UnturnedVehicleAsset vehicle, UnturnedPlayer? player = null)
        {
            player ??= UnturnedPlayer;

            await UniTask.SwitchToMainThread();

            if (await _vehicleSpawner.SpawnVehicleAsync(player, vehicle.VehicleAssetId) == null)
            {
                throw GetLocalizedFriendlyException("Failure", new { VehicleAsset = vehicle });
            }

            await PrintLocalizedAsync("Success", new { VehicleAsset = vehicle });
        }
    }
}