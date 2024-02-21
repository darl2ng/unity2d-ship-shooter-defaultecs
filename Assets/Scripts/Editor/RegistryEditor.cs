using System;
using UnityEditor;
using UnityEngine;
using Registry;
using Editor.Extension;

namespace Editor
{
    public abstract class RegistryEditor<T> : UnityEditor.Editor
    {
        public const string GUID_LOW_FIELD = "_guidLow";
        public const string GUID_HIGH_FIELD = "_guidHigh";
        private SerializedProperty _data;
        private int _arraySize;
        private readonly string _registryDataField;
        private readonly string _entriesField;
        private readonly string _dataField;
        private readonly string _guidField;
        private readonly string _nameField;

        public RegistryEditor(string registryDataField, string entriesField, string dataField, string guidField, string nameField)
        {
            _registryDataField = registryDataField;
            _entriesField = entriesField;
            _dataField = dataField;
            _guidField = guidField;
            _nameField = nameField;
        }

        void OnEnable()
        {
            _data = serializedObject.FindProperty(_entriesField);
            _arraySize = _data.arraySize;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            DrawRegistry(_data, ref _arraySize, _registryDataField, _dataField, _guidField, _nameField);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        public static void DrawRegistry(
            SerializedProperty registry,
            ref int arraySize,
            string registryDataField,
            string dataField,
            string guidField,
            string nameField
        )
        {
            bool wasExpanded = registry.isExpanded;
            registry.isExpanded = EditorGUILayout.Foldout(registry.isExpanded, registryDataField);
            CheckAndHandleExpandedAttribute(registry, wasExpanded, dataField);

            if (registry.isExpanded)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Size");
                arraySize = EditorGUILayout.IntField(arraySize);

                if (GUILayout.Button("Update", GUILayout.Width(60f)))
                {
                    registry.arraySize = arraySize;
                }

                EditorGUILayout.EndHorizontal();

                for (int i = 0; i < registry.arraySize; i++)
                {
                    DrawRegistryEntry(registry, ref arraySize, i, dataField, guidField, nameField);
                }

                EditorGUI.indentLevel--;
            }
        }

        private static void DrawRegistryEntry(
            SerializedProperty registry,
            ref int arraySize,
            int index,
            string dataField,
            string guidField,
            string nameField
        )
        {
            SerializedProperty entry = registry.GetArrayElementAtIndex(index);
            SerializedProperty data = entry.FindPropertyRelative(dataField);
            SerializedProperty name = data.FindPropertyRelative(nameField);

            SerializedProperty guid = entry.FindPropertyRelative(guidField);
            SerializedProperty guidLowValue = guid.FindPropertyRelative(GUID_LOW_FIELD);
            SerializedProperty guidHighValue = guid.FindPropertyRelative(GUID_HIGH_FIELD);
            if (guidLowValue.ulongValue == 0 && guidHighValue.ulongValue == 0)
            {
                SetValue(guid, (SerializableGuid)Guid.NewGuid());
            }

            string displayName = ComputeDisplayName(name?.stringValue, data.displayName);

            bool wasExpanded = entry.isExpanded;
            EditorGUILayout.BeginHorizontal();

            entry.isExpanded = EditorGUILayout.Foldout(entry.isExpanded, displayName);

            CheckAndHandleExpandedAttribute(entry, wasExpanded, dataField);

            if (GUILayout.Button("Duplicate", GUILayout.Width(100)))
            {
                entry.DuplicateCommand();
                arraySize++;
                registry.serializedObject.ApplyModifiedProperties();

                SerializedProperty newEntry = registry.GetArrayElementAtIndex(index + 1);
                var newGuid = newEntry.FindPropertyRelative(guidField);

                SetValue(newGuid, (SerializableGuid)Guid.NewGuid());
            }

            if (GUILayout.Button("Remove", GUILayout.Width(100)))
            {
                entry.DeleteCommand();
                arraySize--;
            }

            EditorGUILayout.EndHorizontal();

            if (!entry.isExpanded)
            {
                return;
            }

            EditorGUI.indentLevel++;

            var guidString = SerializableGuid.Compose(guidLowValue.ulongValue, guidHighValue.ulongValue).ToString();
            DrawRegistryEntry(data, guidString);

            EditorGUI.indentLevel--;
        }

        private static string ComputeDisplayName(string current, string fallback) => !string.IsNullOrEmpty(current) ? current : fallback;

        private static void SetValue(SerializedProperty property, object value)
        {
            Undo.RecordObject(property.serializedObject.targetObject, $"Set {property.name}");
            property.SetValue(value);
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
        private static void DrawRegistryEntry(SerializedProperty data, string label)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(data, new GUIContent(label));
            EditorGUILayout.EndHorizontal();
        }

        public static void CheckAndHandleExpandedAttribute(SerializedProperty property, bool wasExpanded, params string[] relativeProperties)
        {
            if (property == null)
            {
                return;
            }

            if (property.isExpanded == wasExpanded || (Event.current.modifiers & EventModifiers.Alt) != EventModifiers.Alt)
            {
                return;
            }

            HandleExpandedAttribute(property, !wasExpanded, relativeProperties);
        }

        private static void HandleExpandedAttribute(SerializedProperty property, bool isExpanded, params string[] relativeProperties)
        {
            if (property == null)
            {
                return;
            }

            property.isExpanded = isExpanded;
            if (relativeProperties != null)
            {
                foreach (string r in relativeProperties)
                {
                    HandleExpandedAttribute(property.FindPropertyRelative(r), isExpanded);
                }
            }

            if (!property.isArray)
            {
                return;
            }

            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty p = property.GetArrayElementAtIndex(i);
                HandleExpandedAttribute(p, isExpanded, relativeProperties);
            }
        }
    }
}
