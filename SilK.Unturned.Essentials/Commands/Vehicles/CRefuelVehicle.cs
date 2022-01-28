extern alias JetBrainsAnnotations;
using Cysharp.Threading.Tasks;
using JetBrainsAnnotations::JetBrains.Annotations;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Vehicles;
using SDG.Unturned;
using SilK.Unturned.Essentials.Localization;
using System;
using UnityEngine;

namespace SilK.Unturned.Essentials.Commands.Vehicles
{
    [UsedImplicitly]
    [Command("refuelvehicle", Priority = Priority.Normal)]
    [CommandAlias("refuel")]
    [CommandSyntax("[all]")]
    [CommandDescription("Refuels the vehicle you are in or all vehicles.")]
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

            var vehicle = UnturnedPlayer.CurrentVehicle;

            vehicle ??= GetVehicleFromLook();

            if (vehicle == null)
            {
                throw GetLocalizedFriendlyException("NoVehicle");
            }

            vehicle.Vehicle.askFillFuel(ushort.MaxValue);

            await PrintLocalizedAsync("Success", new { Vehicle = vehicle });
        }

        private UnturnedVehicle? GetVehicleFromLook()
        {
            const float maxVehicleDistance = 8;

            var nativePlayer = UnturnedPlayer.Player;

            Physics.Raycast(nativePlayer.look.aim.position, nativePlayer.look.aim.forward, out var raycastHit, maxVehicleDistance,
                RayMasks.VEHICLE);

            if (raycastHit.transform == null)
            {
                return null;
            }

            var vehicle = raycastHit.transform.GetComponent<InteractableVehicle>();

            return vehicle == null ? null : new UnturnedVehicle(vehicle);
        }
    }
}