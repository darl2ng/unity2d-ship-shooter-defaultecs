using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Helper class to handle buttons to entities world until a proper ui data binding system using ecs is implemented.
    /// </summary>
    public sealed class ButtonHelper : MonoBehaviour
    {
        private static readonly Dictionary<string, Action> map = new();

        [SerializeField]
        private string _actionName;

        private Action _action;

        public static void Register(string actionName, Action action) => map.Add(actionName, action);
        public void OnButtonClick() => _action?.Invoke();


        private void Awake()
        {
            if (map.TryGetValue(_actionName, out Action action))
            {
                _action = action;
            }
            else
            {
                Debug.LogError($"ButtonHelper does not find any registered action for {_actionName}");
            }
        }
    }
}
