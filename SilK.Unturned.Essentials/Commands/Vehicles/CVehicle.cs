extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Permissions;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Core.Permissions;
using OpenMod.Extensions.Games.Abstractions.Vehicles;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Vehicles;
using SilK.Unturned.Essentials.Localization;
using System;

namespace SilK.Unturned.Essentials.Commands.Vehicles
{
    [UsedImplicitly]
    [Command("vehicle", Priority = Priority.Normal)]
    [CommandAlias("v")]
    [CommandSyntax("<vehicle> [player]")]
    [CommandDescription("Spawns a vehicle.")]
    [LocalizationSection("Vehicles", "Vehicle")]
    [RegisterCommandPermission(OtherPermission, DefaultGrant = PermissionGrantResult.Deny,
        Description = "Allows spawning of vehicles for other players.")]
    public class CVehicle : EssentialsCommand
    {
        private const string OtherPermission = "other";

        private readonly IVehicleSpawner _vehicleSpawner;

        public CVehicle(IServiceProvider serviceProvider,
            IVehicleSpawner vehicleSpawner) : base(serviceProvider)
        {
            _vehicleSpawner = vehicleSpawner;
        }

        [UsedImplicitly]
        protected async UniTask OnExecuteAsync(UnturnedVehicleAsset vehicle, UnturnedPlayer? player = null)
        {
            if (player != null && await CheckPermissionAsync(OtherPermission) == PermissionGrantResult.Deny)
            {
                throw new NotEnoughPermissionException(Context, OtherPermission);
            }

            player ??= UnturnedPlayer;

            if (await _vehicleSpawner.SpawnVehicleAsync(player, vehicle.VehicleAssetId) == null)
            {
                throw GetLocalizedFriendlyException("Failure", new { VehicleAsset = vehicle });
            }

            await PrintLocalizedAsync("Success", new { VehicleAsset = vehicle });
        }
    }
}