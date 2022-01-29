using OpenMod.API.Permissions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.API.Warps
{
    public static class WarpLocationDirectoryExtensions
    {
        public static async Task<IReadOnlyCollection<IWarpLocation>> GetPermittedWarpLocationsAsync(
            this IWarpLocationDirectory warpLocationDirectory, IPermissionActor permissionActor)
        {
            var warps = await warpLocationDirectory.GetWarpLocationsAsync();

            var permittedWarps = new List<IWarpLocation>();

            foreach (var warp in warps)
            {
                if (await warp.IsPermittedAsync(permissionActor))
                {
                    permittedWarps.Add(warp);
                }
            }

            return permittedWarps;
        }
    }
}
