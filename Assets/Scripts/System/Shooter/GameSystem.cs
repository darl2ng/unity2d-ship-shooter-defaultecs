using UnityEngine;

using DefaultEcs;
using DefaultEcs.System;

using Model.Shooter;

namespace System.Shooter
{
    /// <summary>
    /// Basic game set up with some levels
    /// </summary>
    public sealed class GameSystem : ISystem<float>
    {
        private readonly World _world;
        private readonly EntitySet _set;

        public GameSystem(World world)
        {
            _world = world;
            _set = _world.GetEntities().With<Level>().AsSet();
        }

        public void Update(float state)
        {
            var game = _world.Get<Game>();
            int success = 0;
            int total = 0;
            foreach (var entity in _set.GetEntities())
            {
                total++;
                if (Update(state, in entity, in game))
                    success++;
            }

            if (total == success)
            {
                if (!game.Completed)
                {
                    game.Completed = true;
                    _world.Publish<bool>(true);
                }
            }
        }

        private bool Update(float state, in Entity entity, in Game game)
        {
            var level = entity.Get<Level>();

            if (level == game.ActiveLevel && level.Completed)
            {
                game.ActiveLevel = null;
            }
            if (!level.Completed && game.ActiveLevel == null)
            {
                game.ActiveLevel = level;
            }
            return level.Completed;
        }

        public bool IsEnabled { get; set; }

        public void Dispose() => _set.Dispose();

    }
}
