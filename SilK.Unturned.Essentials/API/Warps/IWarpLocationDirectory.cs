using OpenMod.API.Ioc;
using SilK.Unturned.Essentials.API.Directories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.API.Warps
{
    [Service]
    public interface IWarpLocationDirectory : IElementDirectory<IWarpLocation, IWarpLocationSource>
    {
        Task<IReadOnlyCollection<IWarpLocation>> GetWarpLocationsAsync();
    }
}
