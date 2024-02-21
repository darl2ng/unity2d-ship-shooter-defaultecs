using DefaultEcs;
using DefaultEcs.System;
using Resource;
using Model.Shooter;
using UnityEngine;

namespace System.Shooter
{
    /// <summary>
    /// Basic level set up with enemy spawners
    /// TODO: keep track of killed enemies, completed rounds and winning conditions
    /// </summary>
    public sealed class LevelSystem : AEntitySetSystem<float>
    {
        private readonly LevelRegistry _levelRegistry;
        private readonly Game _game;

        public LevelSystem(World world, LevelRegistry levelRegistry) :
        base(world.GetEntities().With<Level>().AsSet(), true)
        {
            _levelRegistry = levelRegistry;
            _game = world.Get<Game>();
        }

        protected override void Update(float state, in Entity entity)
        {
            var level = entity.Get<Level>();

            if (_game.Completed)
            {
                level.Completed = false;
                level.CurrentRoundIndex = -1;
                level.ActiveRound = null;
                entity.Remove<Round>();
                return;
            }

            if (level == _game.ActiveLevel && !level.Completed)
            {
                if (level.ActiveRound == null || level.ActiveRound.Completed)
                {
                    var entry = _levelRegistry.FirstOrDefaultEntry(search => search.Data.Name == level.Name);
                    if (entry != default && entry.Data.Rounds.Length > 0)
                    {
                        if (level.ActiveRound == null)
                        {
                            var round = new Round() { EnemyCount = entry.Data.Rounds[0].EnemyCount };
                            entity.Set<Round>(round);
                            entity.Set<Spawner>(new Spawner() { Count = round.EnemyCount });
                            level.ActiveRound = round;
                            level.CurrentRoundIndex = 0;
                            Debug.Log($"Started first round of level {level.Name}");
                        }
                        else
                        if (level.ActiveRound.Completed)
                        {
                            level.CurrentRoundIndex++;
                            if (level.CurrentRoundIndex < entry.Data.Rounds.Length)
                            {
                                var round = new Round() { EnemyCount = entry.Data.Rounds[level.CurrentRoundIndex].EnemyCount };
                                entity.Set<Round>(round);
                                entity.Set<Spawner>(new Spawner() { Count = round.EnemyCount });
                                level.ActiveRound = round;
                                Debug.Log($"Started next round {level.CurrentRoundIndex} of level {level.Name}");
                            }
                            else
                            {
                                level.Completed = true;
                                Debug.Log($"Level {level.Name} completed");
                            }
                        }
                    }
                }
            }
        }
    }
}
