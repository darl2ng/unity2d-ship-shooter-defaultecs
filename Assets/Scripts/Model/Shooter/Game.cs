using System;

namespace Model.Shooter
{

    public record Game
    {
        public Level ActiveLevel;
        public bool Completed;
        public string Result;

    }
}