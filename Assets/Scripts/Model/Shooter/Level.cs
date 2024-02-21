using System;

namespace Model.Shooter
{
    [Serializable]
    public record Level : WithName
    {
        /// <summary>
        /// If not zero then level is won if player is still alive
        /// </summary>
        public float WinTimer;

        /// <summary>
        /// If WinTimer is zero then level is won if player has finished this number of rounds
        /// </summary>
        public Round[] Rounds;

        [NonSerialized]
        public Round ActiveRound;

        [NonSerialized]
        public bool Completed;
        
        [NonSerialized]
        public int CurrentRoundIndex = -1;

    }

}