using System.Collections.Generic;
using UnityEngine;

using DefaultEcs;
using DefaultEcs.System;

using Model.UI;
using Transform = Model.UI.Transform;
using Display = Model.UI.Display;

namespace System.UI
{
    /// <summary>
    /// We attach entities to a predefined container per layer and update the position
    /// </summary>
    public sealed class LayeredRenderSystem : AEntityMultiMapSystem<float, Layer>, IDisposable
    {
        private readonly Dictionary<Layer, GameObject> _containerByLayer;

        public LayeredRenderSystem(World world, Dictionary<Layer, GameObject> containerByLayer) :
        base(world.GetEntities().With<Transform>().With<Display>().AsMultiMap<Layer>())
        {
            _containerByLayer = containerByLayer;
        }

        protected override void Update(float state, in Layer key, in Entity entity)
        {
            ref var layer = ref entity.Get<Layer>();
            ref var transform = ref entity.Get<Transform>();
            ref var display = ref entity.Get<Display>();

            if (transform.Enabled)
            {
                if (!display.GameObject.transform.parent)
                {
                    var container = _containerByLayer[layer];
                    display.GameObject.transform.SetParent(container.transform);
                }
                display.GameObject.transform.SetPositionAndRotation(transform.Position, transform.Rotation);
            }
            display.GameObject.SetActive(transform.Enabled);
        }

        public override void Dispose()
        {
            _containerByLayer.Clear();
            base.Dispose();
        }
    }
}
