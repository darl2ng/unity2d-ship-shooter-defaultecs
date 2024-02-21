using System.Collections.Generic;
using UnityEngine;

using DefaultEcs;
using DefaultEcs.System;

using Factory;
using Model.UI;
using Model.Shooter;

using ResourceAsset = Model.Resource.Asset;
using Transform = Model.UI.Transform;
using Screen = Model.UI.Screen;
using UILayer = Model.UI.Layer;
using ColliderLayer = Model.Shooter.Layer;
using Collider2D = Model.Shooter.Collider2D;
using Extension;

namespace System.Shooter
{
    /// <summary>
    /// Basic spawning enemies
    /// </summary>
    public sealed class SpawningSystem : AEntitySetSystem<float>
    {
        private readonly Device _device;
        private readonly IEntityFactory _entityFactory;
        private readonly IAssetFactory _assetFactory;
        private readonly Random _random = new();
        private readonly Func<Vector3>[] _positions;

        public SpawningSystem(World world, IEntityFactory entityFactory, IAssetFactory assetFactory) :
        base(world.GetEntities().With<Level>().With<Round>().With<Spawner>().WhenAdded<Spawner>().AsSet(), true)
        {
            _entityFactory = entityFactory;
            _assetFactory = assetFactory;
            _device = world.Get<Device>();
            _positions = new Func<Vector3>[]{
            () => new Vector3(-2, _random.Next(2, 4), 0),
            () => new Vector3(-1, _random.Next(2, 4), 0),
            () => new Vector3(0, _random.Next(2, 4), 0),
            () => new Vector3(1, _random.Next(2, 4), 0),
            () => new Vector3(2, _random.Next(2, 4), 0)};
        }

        protected override void Update(float state, in Entity entity)
        {
            ref var spawner = ref entity.Get<Spawner>();
            ref var round = ref entity.Get<Round>();
            for (int i = 0; i < spawner.Count; i++)
            {
                var position = _positions[i % _positions.Length]();
                var enemyEntity = _entityFactory.Create<ResourceAsset, Transform, UILayer, Movement, Screen, Enemy, Collider2D, ColliderLayer, Health, Attack>(
                    _assetFactory.GetOrCreate(Enemy.ASSET),
                    new Transform() { Position = position },
                    UILayer.Unit,
                    new Movement() { Power = Enemy.POWER, Velocity = Vector2.down * Enemy.POWER },
                    _device.ActiveScreen,
                    new Enemy(),
                    new Collider2D(Enemy.SIZE, Enemy.OFFSET),
                    ColliderLayer.Enemy,
                    new Health() { Value = Enemy.HEALTH },
                    new Attack(Enemy.DAMAGE_PER_SECOND));

                enemyEntity.SetSameAs<Round>(entity);
            }
            entity.Remove<Spawner>();
        }
    }
}
