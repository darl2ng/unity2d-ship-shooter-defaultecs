using UnityEngine;

namespace Model.Shooter
{
    /// <summary>
    /// Velocity include the full velocity and Power is used to compute Velocity
    /// </summary>
    public record Movement
    {
        public Vector2 Power { get; set; } = Vector2.one;
        public Vector2 Velocity { get; set; } = Vector2.zero;
    }
}
