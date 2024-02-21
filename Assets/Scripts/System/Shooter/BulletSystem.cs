using UnityEngine;
using UnityEngine.InputSystem;

using DefaultEcs;
using DefaultEcs.System;

using Factory;
using Model.Shooter;
using Model.UI;

using ResourceAsset = Model.Resource.Asset;
using UILayer = Model.UI.Layer;
using ColliderLayer = Model.Shooter.Layer;
using Transform = Model.UI.Transform;
using Screen = Model.UI.Screen;
using Collider2D = Model.Shooter.Collider2D;

namespace System.Shooter
{
    /// <summary>
    /// Listen to player action to spawn bullet from player postion.
    /// TODO: split the listen of input system and this via world message
    /// </summary>
    public sealed class BulletSystem : AEntitySetSystem<float>
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IAssetFactory _assetFactory;
        private readonly InputActionReference _firingAction;
        private readonly Device _device;
        private bool _isPressed = false;

        public BulletSystem(World world, IEntityFactory entityFactory, IAssetFactory assetFactory, InputActionReference firingAction) :
        base(world.GetEntities().With<Player>().With<Transform>().AsSet())
        {
            _entityFactory = entityFactory;
            _assetFactory = assetFactory;
            _firingAction = firingAction;
            _device = world.Get<Device>();
        }

        protected override void Update(float state, in Entity entity)
        {
            var action = _firingAction.action;
            if (_device.ActiveScreen.Name == "Menu") return;
            var isPressed = action.IsPressed();

            if (isPressed && !_isPressed)
            {
                _isPressed = true;
            }
            if (!isPressed && _isPressed)
            {
                _isPressed = false;

                ref var transform = ref entity.Get<Transform>();
                var bulletPostion = new Vector3(transform.Position.x, transform.Position.y + Bullet.OFFSET_Y, transform.Position.z);
                _entityFactory.Create<ResourceAsset, Transform, UILayer, Movement, Screen, Bullet, Collider2D, ColliderLayer, Attack>(
                    _assetFactory.GetOrCreate(Bullet.ASSET),
                    new Transform() { Position = bulletPostion },
                    UILayer.Unit,
                    new Movement() { Power = Bullet.POWER, Velocity = Vector2.up * Bullet.POWER },
                    _device.ActiveScreen,
                    new Bullet(),
                    new Collider2D(Bullet.SIZE, Bullet.OFFSET),
                    ColliderLayer.Friend,
                    new Attack(Bullet.DAMAGE_PER_SECOND));
            }
        }
    }
}
