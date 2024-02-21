using UnityEngine;

using Transform = Model.UI.Transform;

namespace Factory
{
    public interface IGameObjectFactory
    {
        GameObject Create(GameObject prefab, Transform transform);
        GameObject Create(GameObject prefab);
        void Destroy(GameObject gameObject);
    }
}
