using System;
using UnityEngine;

namespace Registry
{
    public abstract record RegistryEntry<TIdentifier, TData> : IEquatable<RegistryEntry<TIdentifier, TData>>
    {
        [SerializeField]
        private TIdentifier _guid;
        public TIdentifier Guid => _guid;

        [SerializeField]
        private TData _data;
        public TData Data => _data;

        protected RegistryEntry(TIdentifier guid, TData data)
        {
            _guid = guid;
            _data = data;
        }
    }
}