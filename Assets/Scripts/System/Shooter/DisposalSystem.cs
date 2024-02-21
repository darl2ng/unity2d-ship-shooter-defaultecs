using UnityEngine;

using DefaultEcs;
using DefaultEcs.System;

using Transform = Model.UI.Transform;
using Display = Model.UI.Display;
using Model.Shooter;

namespace System.Shooter
{
    /// <summary>
    /// Dispose entities and game objects when out of screen, except player.
    /// </summary>
    public sealed class DisposalSystem : AEntitySetSystem<float>
    {
        private readonly Camera _camera;
        private readonly float _widthThresoldMin = 0;
        private readonly float _widthThresoldMax = 1;
        private readonly float _heightThresoldMin = 0;
        private readonly float _heightThresoldMax = 1;

        public DisposalSystem(World world, Camera camera) :
        base(world.GetEntities().With<Display>().With<Transform>().Without<Player>().AsSet(), true)
        {
            _camera = camera;
        }

        protected override void Update(float state, in Entity entity)
        {
            ref var transform = ref entity.Get<Transform>();
            if (!transform.Enabled) return;

            Vector2 viewportPosition = _camera.WorldToViewportPoint(transform.Position);

            if (viewportPosition.x > _widthThresoldMax || viewportPosition.x < _widthThresoldMin
            || viewportPosition.y > _heightThresoldMax || viewportPosition.y < _heightThresoldMin)
            {
                entity.Dispose();
            }
        }
    }
}
