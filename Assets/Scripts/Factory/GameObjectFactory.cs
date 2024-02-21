using UnityEngine;

using Transform = Model.UI.Transform;

namespace Factory
{
    /// <summary>
    /// Handle game objects creation and destruction here.
    /// TODO: add pooling
    /// </summary>
    public class GameObjectFactory : IGameObjectFactory
    {
        public GameObject Create(GameObject prefab, Transform transform) => GameObject.Instantiate(prefab, transform.Position, transform.Rotation);

        public GameObject Create(GameObject prefab) => GameObject.Instantiate(prefab);

        public void Destroy(GameObject gameObject) => GameObject.DestroyImmediate(gameObject);

    }
}
