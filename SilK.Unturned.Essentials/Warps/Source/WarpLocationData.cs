using System;
using System.Numerics;

namespace SilK.Unturned.Essentials.Warps.Source
{
    [Serializable]
    public class WarpLocationData
    {
        public string Name { get; set; } = "";

        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public TimeSpan Cooldown { get; set; }

        public bool Equals(WarpLocationData other)
        {
            return Name.Equals(other.Name)
                   && Position.Equals(other.Position)
                   && Rotation.Equals(other.Rotation)
                   && Cooldown.Equals(other.Cooldown);
        }
    }
}
