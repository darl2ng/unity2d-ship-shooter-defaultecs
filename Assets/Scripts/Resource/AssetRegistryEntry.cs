using System;

using Registry;

namespace Resource
{
    [Serializable]
    public sealed record AssetRegistryEntry : RegistryEntry<SerializableGuid, AssetReference>
    {
        public AssetRegistryEntry(SerializableGuid guid, AssetReference asset) : base(guid, asset) { }

        public const string DATA_FIELD = "_data";
        public const string GUID_FIELD = "_guid";
    }
}
