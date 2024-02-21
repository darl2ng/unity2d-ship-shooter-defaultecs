using UnityEngine;

using DefaultEcs;
using DefaultEcs.System;

using Transform = Model.UI.Transform;
using Movement = Model.Shooter.Movement;

namespace System.Shooter
{
    /// <summary>
    /// Basic movement system that update game objects using entity velocity
    /// </summary>
    public sealed class MovementSystem : AEntitySetSystem<float>
    {
        public MovementSystem(World world) :
        base(world.GetEntities().With<Transform>().With<Movement>().AsSet())
        { }

        protected override void Update(float state, in Entity entity)
        {
            ref var movement = ref entity.Get<Movement>();
            ref var transform = ref entity.Get<Transform>();
            Vector2 offset = movement.Velocity * state;
            Vector3 vector3 = new(offset.x, offset.y, 0);
            transform.Position += vector3;
        }
    }
}
