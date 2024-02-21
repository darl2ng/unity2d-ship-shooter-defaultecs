using UnityEngine;

namespace Model.UI
{
    public record Transform
    {
        public bool Enabled { get; set; } = true;
        public Vector3 Position { get; set; } = Vector3.zero;
        public Quaternion Rotation { get; set; } = Quaternion.identity;
    }
}
