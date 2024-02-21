using UnityEngine;

using Registry;
using Model.Shooter;

namespace Resource
{
    [CreateAssetMenu(fileName = "LevelRegistry", menuName = "Registries/Level Registry")]
    public sealed class LevelRegistry : Registry<LevelRegistryEntry, SerializableGuid, Level>
    {
        public const string REGISTRY_DATA_FIELD = "Levels";
        public const string ENTRIES_FIELD = "_entries";
    }
}
