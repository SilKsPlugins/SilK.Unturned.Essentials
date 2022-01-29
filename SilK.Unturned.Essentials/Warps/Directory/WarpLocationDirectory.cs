using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
using OpenMod.API.Prioritization;
using SilK.Unturned.Essentials.API.Directories;
using SilK.Unturned.Essentials.API.Warps;
using SilK.Unturned.Essentials.Directories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Warps.Directory
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton, Priority = Priority.Lowest)]
    public class WarpLocationDirectory : DefaultElementDirectory<IWarpLocationSource>, IWarpLocationDirectory
    {
        public Task<IReadOnlyCollection<IWarpLocation>> GetWarpLocationsAsync()
        {
            return this.GetContentsAsync();
        }
    }
}
