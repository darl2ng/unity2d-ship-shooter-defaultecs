using System.Collections.Generic;

using Model.Resource;
using Resource;

namespace Factory
{
    /// <summary>
    /// Asset factory to factilitate and control creation of asset elements
    /// TODO: use asset registries to pre-validate assets
    /// </summary>
    public sealed class AssetFactory : IAssetFactory
    {
        private readonly Dictionary<string, Asset> _lookup = new();
        private readonly AssetRegistry[] _assetRegistries;

        public AssetFactory(AssetRegistry[] assetRegistries) => _assetRegistries = assetRegistries;


        public bool TryCreate(string name, out Asset asset)
        {
            if (_lookup.ContainsKey(name))
            {
                asset = default;
                return false;
            }

            asset = new Asset() { Name = name };
            return true;
        }

        public Asset GetOrCreate(string name)
        {
            if (_lookup.ContainsKey(name))
            {
                return _lookup[name];
            }

            var asset = new Asset() { Name = name };
            _lookup[name] = asset;
            return asset;
        }
    }
}
