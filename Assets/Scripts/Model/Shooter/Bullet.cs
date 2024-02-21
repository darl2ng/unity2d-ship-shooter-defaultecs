using UnityEngine;

namespace Model.Shooter
{
    /// <summary>
    /// TODO: move the parameters here into registries for edition
    /// </summary>
    public record Bullet
    {
        public const string ASSET = "Bullet";
        public const float OFFSET_Y = 0.5f;
        public const float DAMAGE_PER_SECOND = 5f;
        public static Vector2 POWER = Vector2.one;
        public static Vector2 SIZE = Vector2.one;
        public static Vector2 OFFSET = Vector2.one;
    }

}