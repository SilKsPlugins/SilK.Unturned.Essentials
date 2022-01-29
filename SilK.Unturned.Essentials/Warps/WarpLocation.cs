using OpenMod.API;
using OpenMod.API.Permissions;
using OpenMod.Core.Cooldowns;
using SilK.Unturned.Essentials.API.Warps;
using SilK.Unturned.Essentials.Warps.Source;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Warps
{
    public class WarpLocation : IWarpLocation
    {
        private readonly IPermissionChecker _permissionChecker;
        private readonly IOpenModComponent _component;
        private readonly IPermissionRegistry _permissionRegistry;
        private readonly ICommandCooldownStore _cooldownStore;

        public WarpLocation(WarpLocationData warpLocationData,
            IPermissionChecker permissionChecker,
            IOpenModComponent component,
            IPermissionRegistry permissionRegistry,
            ICommandCooldownStore cooldownStore)
        {
            WarpLocationData = warpLocationData;

            Name = warpLocationData.Name;
            Position = warpLocationData.Position;
            Rotation = warpLocationData.Rotation;
            Cooldown = warpLocationData.Cooldown;

            _permissionChecker = permissionChecker;
            _component = component;
            _permissionRegistry = permissionRegistry;
            _cooldownStore = cooldownStore;
        }

        public WarpLocationData WarpLocationData { get; }

        public string Name { get; }

        public Vector3 Position { get; }

        public Quaternion Rotation { get; }

        public TimeSpan Cooldown { get; }

        protected virtual string GetPermission()
        {
            return "commands." + Name.ToLower();
        }

        protected virtual string GetDescription()
        {
            return $"Allows warping to the {Name} warp.";
        }

        public virtual void RegisterPermission()
        {
            var permission = GetPermission();
            var description = GetDescription();

            if (_permissionRegistry.FindPermission(_component, permission) != null)
            {
                return;
            }

            _permissionRegistry.RegisterPermission(_component, permission, description, PermissionGrantResult.Deny);
        }

        public virtual async Task<bool> IsPermittedAsync(IPermissionActor actor)
        {
            var permission = GetPermission();

            return await _permissionChecker.CheckPermissionAsync(actor, permission) == PermissionGrantResult.Grant;
        }
    }
}
