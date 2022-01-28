extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Games.Abstractions.Vehicles;
using OpenMod.Unturned.Vehicles;
using SilK.Unturned.Essentials.Localization;
using System;
using System.Numerics;

namespace SilK.Unturned.Essentials.Commands.Vehicles
{
    [UsedImplicitly]
    [Command("spawnvehicle", Priority = Priority.Normal)]
    [CommandAlias("spawnveh")]
    [CommandSyntax("<x> <y> <z> <vehicle>")]
    [CommandDescription("Spawns a vehicle at the specified position.")]
    [LocalizationSection("Vehicles", "SpawnVehicle")]
    public class CSpawnVehicle : EssentialsCommand
    {
        private readonly IVehicleSpawner _vehicleSpawner;

        public CSpawnVehicle(IServiceProvider serviceProvider,
            IVehicleSpawner vehicleSpawner) : base(serviceProvider)
        {
            _vehicleSpawner = vehicleSpawner;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(float x, float y, float z, UnturnedVehicleAsset vehicle)
        {
            var position = new Vector3(x, y, z);

            if (await _vehicleSpawner.SpawnVehicleAsync(position, vehicle.VehicleAssetId) == null)
            {
                throw GetLocalizedFriendlyException("Failure", new { X = x, Y = y, Z = z, VehicleAsset = vehicle });
            }

            await PrintLocalizedAsync("Success", new { X = x, Y = y, Z = z, VehicleAsset = vehicle });
        }
    }
}