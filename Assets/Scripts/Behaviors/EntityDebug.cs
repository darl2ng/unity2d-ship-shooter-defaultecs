using UnityEngine;

using DefaultEcs;

namespace Behaviors
{
    public sealed class EntityDebug : MonoBehaviour
    {
        private Entity _entity;

        [SerializeField]
        private string _description;

        public void SetEntity(Entity entity)
        {
            _entity = entity;
        }

        private void Update()
        {
            _description = $"{_entity}: {_entity.IsAlive} {_entity.IsEnabled()}";
        }
    }
}
