using OpenMod.Unturned.Players;
using OpenMod.Unturned.Vehicles;
using SDG.Unturned;
using UnityEngine;

namespace SilK.Unturned.Essentials.Helpers
{
    public static class RaycastHelper
    {
        public static UnturnedVehicle? GetVehicleFromLook(UnturnedPlayer player)
        {
            const float maxVehicleDistance = 8;

            var aim = player.Player.look.aim;

            Physics.Raycast(aim.position, aim.forward, out var raycastHit, maxVehicleDistance, RayMasks.VEHICLE);

            if (raycastHit.transform == null)
            {
                return null;
            }

            var vehicle = raycastHit.transform.GetComponent<InteractableVehicle>();

            return vehicle == null ? null : new UnturnedVehicle(vehicle);
        }
    }
}
