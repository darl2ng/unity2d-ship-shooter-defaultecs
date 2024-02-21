using System;

using Registry;
using Model.Shooter;

namespace Resource
{
    [Serializable]
    public sealed record LevelRegistryEntry : RegistryEntry<SerializableGuid, Level>
    {
        public LevelRegistryEntry(SerializableGuid guid, Level level) : base(guid, level) { }

        public const string DATA_FIELD = "_data";
        public const string GUID_FIELD = "_guid";
    }
}
