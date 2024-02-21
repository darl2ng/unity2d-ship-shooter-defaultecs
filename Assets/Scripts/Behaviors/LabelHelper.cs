using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Helper class to render a dynamic text from entities world until a proper ui data binding system using ecs is implemented.
    /// </summary>
    public sealed class LabelHelper : MonoBehaviour
    {
        private static readonly Dictionary<string, Func<string>> map = new();

        [SerializeField]
        private string _labelName;

        [SerializeField]
        private TextMeshProUGUI _text;

        private Func<string> _value;

        public static void Register(string labelName, Func<string> value) => map.Add(labelName, value);

        private void Awake()
        {
            if (map.TryGetValue(_labelName, out Func<string> value))
            {
                _value = value;
            }
            else
            {
                Debug.LogError($"LabelHelper does not find any registered value for {_labelName}");
            }
        }

        private void Update()
        {
            if (_text != null && _value != null) _text.text = _value.Invoke();
        }
    }
}
