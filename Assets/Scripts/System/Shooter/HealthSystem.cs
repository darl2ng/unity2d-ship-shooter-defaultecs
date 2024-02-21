using DefaultEcs;
using DefaultEcs.System;

using Model.Shooter;
using Unity.VisualScripting;
using Collision = Model.Shooter.Collision;

namespace System.Shooter
{
    /// <summary>
    /// Dispose entities when health is depleted.
    /// </summary>
    public sealed class HealthSystem : AEntitySetSystem<float>
    {
        public HealthSystem(World world) :
        base(world.GetEntities().With<Health>().Without<Collision>().AsSet(), true)
        {
        }

        protected override void Update(float state, in Entity entity)
        {
            ref var health = ref entity.Get<Health>();
            if (health.Value <= 0)
            {
                if (entity.Has<Enemy>())
                {
                    ref var score = ref World.Get<Score>();
                    score.Value++;
                    score.MaxValue++;
                    entity.Dispose();
                }

                if (entity.Has<Player>())
                {
                    ref var lives = ref World.Get<Lives>();
                    lives.Value--;
                    health.Value = Player.HEALTH;
                    if (lives.Value <= 0)
                    {
                        if (!World.Get<Game>().Completed)
                            World.Publish<bool>(false);
                    }
                }
            }
        }
    }
}
