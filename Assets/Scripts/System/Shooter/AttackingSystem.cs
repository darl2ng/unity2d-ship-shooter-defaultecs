using DefaultEcs;
using DefaultEcs.System;

using Model.Shooter;
using Collision = Model.Shooter.Collision;

namespace System.Shooter
{
    /// <summary>
    /// Applying attack on health when a collision happening using shared components, until health is depleted.
    /// TODO: Pooling
    /// </summary>
    public sealed class AttackingSystem : AEntitySetSystem<float>
    {
        public AttackingSystem(World world) :
        base(world.GetEntities().With<Collision>().With<Attack>().With<Health>().AsSet(), true)
        { }

        protected override void Update(float state, in Entity entity)
        {
            ref var health = ref entity.Get<Health>();
            ref var attack = ref entity.Get<Attack>();
            health.Value -= attack.DamagePerSecond * state;
            entity.Dispose();
        }
    }
}
