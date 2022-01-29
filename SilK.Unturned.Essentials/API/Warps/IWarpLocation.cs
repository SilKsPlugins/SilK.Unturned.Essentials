using OpenMod.API.Permissions;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.API.Warps
{
    public interface IWarpLocation
    {
        string Name { get; }

        Vector3 Position { get; }

        Quaternion Rotation { get; }

        TimeSpan Cooldown { get; }

        Task<bool> IsPermittedAsync(IPermissionActor actor);
    }
}
