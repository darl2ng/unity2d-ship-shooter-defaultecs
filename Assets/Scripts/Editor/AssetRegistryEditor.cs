using UnityEditor;
using Resource;

namespace Editor
{
    [CustomEditor(typeof(AssetRegistry))]
    public sealed class AssetRegistryEditor : RegistryEditor<AssetReference>
    {
        public AssetRegistryEditor() : base(
            AssetRegistry.REGISTRY_DATA_FIELD,
            AssetRegistry.ENTRIES_FIELD,
            AssetRegistryEntry.DATA_FIELD,
            AssetRegistryEntry.GUID_FIELD,
            nameof(AssetReference.Name))
        { }
    }
}
