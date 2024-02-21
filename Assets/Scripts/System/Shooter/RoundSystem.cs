using UnityEngine;

using DefaultEcs;
using DefaultEcs.System;

using Model.Shooter;
using DefaultEcs.Command;

namespace System.Shooter
{
    /// <summary>
    /// Basic ending condition for game and its levels
    /// </summary>
    public sealed class RoundSystem : ISystem<float>
    {
        private readonly World _world;
        private readonly EntityCommandRecorder _recorder;
        private readonly EntitySet _setLevels;
        private readonly EntitySet _setPlayers;
        private readonly EntitySet _setEnemies;
        private readonly EntitySet _setBullets;
        private readonly Game _game;

        public RoundSystem(World world, EntityCommandRecorder recorder)
        {
            _world = world;
            _recorder = recorder;
            _setLevels = _world.GetEntities().With<Level>().With<Round>().AsSet();
            _setEnemies = _world.GetEntities().With<Enemy>().With<Round>().AsSet();
            _setBullets = world.GetEntities().With<Bullet>().AsSet();
            _setPlayers = world.GetEntities().With<Player>().AsSet();
            _game = world.Get<Game>();
        }

        public void Update(float state)
        {
            if (_setPlayers.GetEntities().Length == 0)
            {
                if (!_game.Completed)
                {
                    _game.Completed = true;

                    foreach (var entity in _setEnemies.GetEntities())
                    {
                        _recorder.Record(entity).Dispose();
                    }
                    foreach (var entity in _setBullets.GetEntities())
                    {
                        _recorder.Record(entity).Dispose();
                    }
                    _world.Publish<bool>(false);
                }
                return;
            }

            foreach (var entity in _setLevels.GetEntities())
            {
                Update(state, in entity);
            }
        }

        private void Update(float state, in Entity entity)
        {
            ref var round = ref entity.Get<Round>();
            if (round.Completed) return;
            if (!round.Started && _setEnemies.GetEntities().Length > 0)
            {
                Debug.Log("Round is started !");
                round.Started = true;
            }
            if (round.Started && _setEnemies.GetEntities().Length == 0)
            {
                Debug.Log("All enemies gone, to decide here !");
                round.Completed = true;
            }
        }

        public void Dispose()
        {
            _setLevels.Dispose();
            _setEnemies.Dispose();
            _setPlayers.Dispose();
        }

        public bool IsEnabled { get; set; }
    }
}
