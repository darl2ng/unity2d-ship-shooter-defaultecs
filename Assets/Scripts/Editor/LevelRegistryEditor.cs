using UnityEditor;
using Resource;
using Model.Shooter;

namespace Editor
{
    [CustomEditor(typeof(LevelRegistry))]
    public sealed class LevelRegistryEditor : RegistryEditor<Level>
    {
        public LevelRegistryEditor() : base(
            LevelRegistry.REGISTRY_DATA_FIELD,
            LevelRegistry.ENTRIES_FIELD,
            LevelRegistryEntry.DATA_FIELD, 
            LevelRegistryEntry.GUID_FIELD, 
            nameof(Level.Name))
        { }
    }
}