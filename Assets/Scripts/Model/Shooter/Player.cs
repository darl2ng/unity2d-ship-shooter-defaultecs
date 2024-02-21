using UnityEngine;

namespace Model.Shooter
{
    /// <summary>
    /// TODO: move the parameters here into registries for edition
    /// </summary>
    public record Player()
    {
        public const string ASSET = "Player";
        public static Vector2 POWER = Vector2.one;
        public static Vector2 SIZE = Vector2.one;
        public static Vector2 OFFSET = Vector2.one;
        public const float HEALTH = 25f;
    }
}