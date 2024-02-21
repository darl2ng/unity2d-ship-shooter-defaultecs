using UnityEngine;

using DefaultEcs;
using DefaultEcs.System;

using Behaviors;
using Factory;
using Registry;
using Resource;
using ResourceAsset = Model.Resource.Asset;
using Display = Model.UI.Display;

namespace System.Asset
{
    /// <summary>
    /// For all entities with valid component, create a entity display that holds an actual game object with the declared visual.
    /// Once the entity display is constructed, remove the entity asset which is considered a asset request component.
    /// Finally destroy the created game object when the entity is disposed.
    /// </summary>
    public sealed class AssetSystem : AEntitySetSystem<float>
    {
        private readonly AssetRegistry[] _assetRegistries;
        private readonly IGameObjectFactory _gameObjectFactory;

        public AssetSystem(World world, AssetRegistry[] assetRegistries, IGameObjectFactory gameObjectFactory) :
        base(world.GetEntities().With<ResourceAsset>().WhenAdded<ResourceAsset>().AsSet(), true)
        {
            _assetRegistries = assetRegistries;
            _gameObjectFactory = gameObjectFactory;
            world.SubscribeEntityDisposed(OnEntityDisposed);
        }

        protected override void Update(float state, in Entity entity)
        {
            ref var asset = ref entity.Get<ResourceAsset>();
            var assetReference = GetAssetReference(asset);

            if (assetReference != null && assetReference.Prefab != null)
            {
                var go = _gameObjectFactory.Create(assetReference.Prefab);
                go.AddComponent<EntityDebug>().SetEntity(entity);
                var display = new Display() { GameObject = go };
                entity.Set<Display>(display);
                entity.Remove<ResourceAsset>();
            }
            else
            {
                entity.Remove<ResourceAsset>();
                Debug.LogError($"Asset invalid with name '{asset.Name}' and Guid '{asset.Guid}'");
            }
        }

        private AssetReference GetAssetReference(ResourceAsset asset)
        {
            if (asset.Guid != SerializableGuid.Empty)
            {
                foreach (var assetRegistry in _assetRegistries)
                {
                    if (assetRegistry.HasEntry(asset.Guid))
                    {
                        return assetRegistry[asset.Guid];
                    }
                }
                return null;
            }

            if (!string.IsNullOrEmpty(asset.Name))
            {
                foreach (var assetRegistry in _assetRegistries)
                {
                    AssetRegistryEntry entry = assetRegistry.FirstOrDefaultEntry(entry => entry.Data.Name == asset.Name);
                    if (entry != null)
                    {
                        return entry.Data;
                    }
                }
            }

            return null;
        }

        private void OnEntityDisposed(in Entity entity)
        {
            if (entity.Has<Display>())
            {
                ref var display = ref entity.Get<Display>();
                _gameObjectFactory.Destroy(display.GameObject);
            }
        }
    }
}
