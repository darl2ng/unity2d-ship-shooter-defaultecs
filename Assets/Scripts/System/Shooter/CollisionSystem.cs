using UnityEngine;

using DefaultEcs;
using DefaultEcs.System;

using Transform = Model.UI.Transform;
using Collider2D = Model.Shooter.Collider2D;
using Layer = Model.Shooter.Layer;

namespace System.Shooter
{
    /// <summary>
    /// Basic collision detection to call a handler. Only different collision layers can collide.
    /// Example of handler is to create a new entity with shared components to modelize collision of 2 entities.
    /// TODO: handle movement to take into account moving aspect.
    /// </summary>
    public sealed class CollisionSystem : AEntityMultiMapSystem<float, Layer>
    {
        private readonly Action<Entity, Entity> _action;

        public CollisionSystem(World world, Action<Entity, Entity> action) :
        base(world.GetEntities().With<Transform>().With<Collider2D>().With<Layer>().AsMultiMap<Layer>())
        {
            _action = action;
        }

        protected override void Update(float state, in Layer key, in Entity entity)
        {
            ref var transform = ref entity.Get<Transform>();
            ref var colider2D = ref entity.Get<Collider2D>();

            Rect bound = new(transform.Position.x, transform.Position.y, colider2D.Size.x, colider2D.Size.x);

            foreach (var layer in MultiMap.Keys)
            {
                if (layer == key) continue;

                if (!MultiMap.TryGetEntities(layer, out var entities))
                {
                    continue;
                }

                foreach (ref readonly var entityToCheck in entities)
                {
                    ref var transformToCheck = ref entityToCheck.Get<Transform>();
                    ref var collider2DToCheck = ref entityToCheck.Get<Collider2D>();

                    Rect boundToCheck = new(transformToCheck.Position.x, transformToCheck.Position.y, collider2DToCheck.Size.x, colider2D.Size.y);
                    if (bound.Overlaps(boundToCheck))
                    {
                        _action?.Invoke(entity, entityToCheck);
                    }
                }
            }
        }
    }
}
