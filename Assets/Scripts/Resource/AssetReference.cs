using System;
using UnityEngine;

namespace Resource
{
    [Serializable]
    public record AssetReference
    {
        public string Name;
        public GameObject Prefab;

    }
}
