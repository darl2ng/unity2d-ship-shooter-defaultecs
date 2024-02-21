using DefaultEcs;
using DefaultEcs.System;

using Model.UI;
using Screen = Model.UI.Screen;
using Transform = Model.UI.Transform;

namespace System.Asset
{
    /// <summary>
    /// Basic implementation of grouping entities to display using Screen.
    /// TODO: subsribe to set screen message to update the active screen here.
    /// </summary>
    public sealed class ScreenSystem : AEntitySetSystem<float>
    {
        private readonly Device _device;

        public ScreenSystem(World world) :
        base(world.GetEntities().With<Screen>().With<Transform>().AsSet())
        {
            _device = world.Get<Device>();
        }

        protected override void Update(float state, in Entity entity)
        {
            ref var screen = ref entity.Get<Screen>();
            ref var transform = ref entity.Get<Transform>();
            transform.Enabled = screen == _device.ActiveScreen;
        }


    }
}
