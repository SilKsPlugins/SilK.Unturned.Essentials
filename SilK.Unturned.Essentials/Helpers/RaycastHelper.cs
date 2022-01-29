using OpenMod.UnityEngine.Extensions;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Vehicles;
using SDG.Unturned;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace SilK.Unturned.Essentials.Helpers
{
    public static class RaycastHelper
    {
        public static RaycastHit GetRaycastFromAim(UnturnedPlayer player, float maxDistance, int layerMask, out bool success)
        {
            var aim = player.Player.look.aim;

            success = Physics.Raycast(aim.position, aim.forward, out var raycastHit, maxDistance, layerMask);

            return raycastHit;
        }

        public static RaycastHit GetRaycastFromAim(UnturnedPlayer player, float maxDistance, int layerMask)
        {
            return GetRaycastFromAim(player, maxDistance, layerMask, out _);
        }

        public static UnturnedVehicle? GetVehicleFromLook(UnturnedPlayer player)
        {
            const float maxVehicleDistance = 8;

            var raycast = GetRaycastFromAim(player, maxVehicleDistance, LayerMasks.VEHICLE);

            var vehicle = raycast.transform.GetComponent<InteractableVehicle>();

            return vehicle == null ? null : new UnturnedVehicle(vehicle);
        }

        public static Vector3? GetLookPosition(UnturnedPlayer player, float maxDistance)
        {
            var aim = player.Player.look.aim;

            var raycast = GetRaycastFromAim(player, maxDistance, RayMasks.BLOCK_COLLISION & ~(1 << 0x15),
                out var success);

            return success ? raycast.point.ToSystemVector() : null;
        }
    }
}
