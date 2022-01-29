using OpenMod.API.Commands;
using SilK.Unturned.Essentials.API.Warps;
using SilK.Unturned.Essentials.Helpers;
using System;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Warps
{
    public class WarpLocationCommandParameterResolveProvider : ICommandParameterResolveProvider
    {
        private readonly IWarpLocationDirectory _warpLocationDirectory;

        public WarpLocationCommandParameterResolveProvider(IWarpLocationDirectory warpLocationDirectory)
        {
            _warpLocationDirectory = warpLocationDirectory;
        }

        public bool Supports(Type type)
        {
            return type == typeof(IWarpLocation) || type == typeof(WarpLocation);
        }

        public async Task<object?> ResolveAsync(Type type, string input)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!Supports(type))
            {
                throw new ArgumentException("The given type is not supported", nameof(type));
            }

            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var warps = await _warpLocationDirectory.GetWarpLocationsAsync();

            var warp = warps.FindBestMatch(input, warpLocation => warpLocation.Name);

            if (type == typeof(IWarpLocation))
            {
                return warp;
            }

            if (type == typeof(WarpLocation))
            {
                return warp as WarpLocation;
            }

            throw new InvalidOperationException($"Unable to return type {type}");
        }
    }
}
