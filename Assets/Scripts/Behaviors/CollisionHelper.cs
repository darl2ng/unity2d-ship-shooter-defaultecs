using System;
using UnityEngine;

using DefaultEcs;

namespace Behaviors
{
    /// <summary>
    /// Helper class until a proper collision system using ecs is implemented.
    /// </summary>
    [Obsolete]
    public sealed class CollisionHelper : MonoBehaviour
    {
        private Action<CollisionHelper, CollisionHelper> _action;

        [SerializeField]
        private bool _isTrigger = false;
        public bool IsTrigger => _isTrigger;

        private bool _isEntitySet = false;
        private Entity _entity;

        public bool IsEntitySet => _isEntitySet;

        public Entity Entity => _entity;

        public void SetEntity(in Entity entity)
        {
            _isEntitySet = true;
            _entity = entity;
        }

        public void SetAction(Action<CollisionHelper, CollisionHelper> action)
        {
            _action = action;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isTrigger)
            {
                _action?.Invoke(this, other.gameObject.GetComponent<CollisionHelper>());
            }
        }
    }
}
