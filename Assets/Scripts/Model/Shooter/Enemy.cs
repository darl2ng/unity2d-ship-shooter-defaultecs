using UnityEngine;

namespace Model.Shooter
{
    /// <summary>
    /// TODO: move the parameters here into registries for edition
    /// </summary>
    public record Enemy
    {
        public const string ASSET = "Enemy";
        public const float HEALTH = 5f;
        public static Vector2 POWER = Vector2.one;
        public static Vector2 SIZE = Vector2.one;
        public static Vector2 OFFSET = Vector2.one;
        public const float DAMAGE_PER_SECOND = 15f;
    }

}