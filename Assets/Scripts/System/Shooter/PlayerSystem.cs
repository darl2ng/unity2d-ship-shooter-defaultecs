using UnityEngine;
using UnityEngine.InputSystem;

using DefaultEcs;
using DefaultEcs.System;

using Model.Shooter;
using Transform = Model.UI.Transform;
using Display = Model.UI.Display;

namespace System.Shooter
{
    /// <summary>
    /// Listen to player action to update player entity movement.
    /// TODO: split the listen of input system and this via world message
    /// </summary>
    public sealed class PlayerSystem : AEntitySetSystem<float>
    {
        private readonly InputActionReference _movementAction;

        public PlayerSystem(World world, InputActionReference movementAction) :
        base(world.GetEntities().With<Player>().With<Transform>().With<Display>().AsSet())
        {
            _movementAction = movementAction;
        }

        protected override void Update(float state, in Entity entity)
        {
            var value = _movementAction.action.ReadValue<Vector2>();
            ref var movement = ref entity.Get<Movement>();
            movement.Velocity = movement.Power * value;
        }
    }
}
