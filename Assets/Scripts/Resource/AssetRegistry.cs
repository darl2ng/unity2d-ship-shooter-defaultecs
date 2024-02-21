using UnityEngine;

using Registry;

namespace Resource
{
    [CreateAssetMenu(fileName = "AssetRegistry", menuName = "Registries/Asset Registry")]
    public sealed class AssetRegistry : Registry<AssetRegistryEntry, SerializableGuid, AssetReference>
    {
        public const string REGISTRY_DATA_FIELD = "Entries";
        public const string ENTRIES_FIELD = "_entries";
    }
}
