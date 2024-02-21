using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Editor.Extension
{
    /// <summary>
    /// Helper to parse, get and set values for serialized object. 
    /// Setting won't work for value object.
    /// </summary>
    public static class SerializedPropertyExtension
    {
        private struct PropertyPathComponent
        {
            public string propertyName;
            public int elementIndex;
        }

        private static readonly Regex arrayElementRegex = new(@"\GArray\.data\[(\d+)\]", RegexOptions.Compiled);

        public static IEnumerable<SerializedProperty> FindChildrenProperties(this SerializedProperty parent, int depth = 1)
        {
            int depthOfParent = parent.depth;
            IEnumerator enumerator = parent.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current is not SerializedProperty childProperty) continue;
                if (childProperty.depth > depthOfParent + depth) continue;

                yield return childProperty.Copy();
            }
        }

        public static object GetValue(this SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            object value = property.serializedObject.targetObject;
            int index = 0;
            while (ParsePathComponent(propertyPath, ref index, out var component))
                value = GetPathComponentValue(ref value, component);
            return value;
        }

        public static bool SetValue(this SerializedProperty property, object value)
        {
            string propertyPath = property.propertyPath;
            object container = property.serializedObject.targetObject;

            int index = 0;
            ParsePathComponent(propertyPath, ref index, out var component);
            while (ParsePathComponent(propertyPath, ref index, out var nextComponent))
            {
                container = GetPathComponentValue(ref container, component);
                component = nextComponent;
            }

            SetPathComponentValue(ref container, component, value);
            return property.serializedObject.ApplyModifiedProperties();
        }

        private static bool ParsePathComponent(string propertyPath, ref int index, out PropertyPathComponent component)
        {
            component = new PropertyPathComponent();

            if (index >= propertyPath.Length)
                return false;

            var arrayElementMatch = arrayElementRegex.Match(propertyPath, index);
            if (arrayElementMatch.Success)
            {
                index += arrayElementMatch.Length + 1;
                component.elementIndex = int.Parse(arrayElementMatch.Groups[1].Value);
                return true;
            }

            int dotIndex = propertyPath.IndexOf('.', index);
            if (dotIndex == -1)
            {
                component.propertyName = propertyPath[index..];
                index = propertyPath.Length;
            }
            else
            {
                component.propertyName = propertyPath[index..dotIndex];
                index = dotIndex + 1;
            }

            return true;
        }

        private static object GetPathComponentValue(ref object container, PropertyPathComponent component)
        {
            if (component.propertyName == null)
            {
                IList list = (IList)container;
                return list.Count > 0 ? list[component.elementIndex] : null;
            }
            else
                return GetMemberValue(ref container, component.propertyName);
        }

        private static void SetPathComponentValue(ref object container, PropertyPathComponent component, object value)
        {
            if (component.propertyName == null)
                ((IList)container)[component.elementIndex] = value;
            else
                SetMemberValue(ref container, component.propertyName, value);
        }

        private static object GetMemberValue(ref object container, string name)
        {
            if (container == null)
                return null;

            Type type = container.GetType();

            MemberInfo[] members = GetMembers(type, name);

            foreach (MemberInfo member in members)
            {
                if (member is FieldInfo field)
                    return field.GetValue(container);
                else if (member is PropertyInfo property)
                    return property.GetValue(container);
            }

            return null;
        }

        private static void SetMemberValue(ref object container, string name, object value)
        {
            if (container == null)
                return;

            Type type = container.GetType();
            MemberInfo[] members = GetMembers(type, name);

            foreach (MemberInfo member in members)
            {
                if (member is FieldInfo field)
                {
                    field.SetValue(container, value);
                    return;
                }
                else if (member is PropertyInfo property)
                {
                    property.SetValue(container, value);
                    return;
                }
            }

            Debug.Assert(false, $"Failed to set member {container}.{name} via reflection");
        }

        private static MemberInfo[] GetMembers(Type type, string name)
        {
            MemberInfo[] members = type.GetMember(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            while (members.Length == 0 && type.BaseType != null)
            {
                members = GetMembers(type.BaseType, name);
            }

            return members;
        }
    }
}
