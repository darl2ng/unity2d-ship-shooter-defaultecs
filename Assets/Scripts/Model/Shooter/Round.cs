using System;

namespace Model.Shooter
{
    [Serializable]
    public record Round
    {
        public int EnemyCount;

        [NonSerialized]
        public bool Started;

        [NonSerialized]
        public bool Completed;

    }
}