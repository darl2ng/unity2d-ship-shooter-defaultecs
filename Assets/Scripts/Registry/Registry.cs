using System;
using System.Collections.Generic;

using UnityEngine;

using Extension;

namespace Registry
{   
    public abstract class Registry<TEntry, TIdentifier, TData> : ScriptableObject, IRegistry, IDisposable
        where TEntry : RegistryEntry<TIdentifier, TData>
    {
        [SerializeField]
        private List<TEntry> _entries = new();

        private readonly Dictionary<TIdentifier, TEntry> _entryById = new();

        public TData this[TIdentifier id] => GetEntry(id);

        public bool HasEntry(TIdentifier id)
        {
            if (_entries == null)
            {
                return false;
            }

            return _entryById.ContainsKey(id);
        }

        public TData GetEntry(TIdentifier id)
        {
            if (TryGetEntry(id, out TData value))
            {
                return value;
            }
            else
            {
                return default;
            }
        }

        public bool TryGetEntry(TIdentifier id, out TData value)
        {
            if (_entries == null)
            {
                value = default;
                return false;
            }

            if (!_entryById.TryGetValue(id, out TEntry entry))
            {
                foreach (TEntry searchEntry in _entries)
                {
                    if (searchEntry.Guid.Equals(id))
                    {
                        entry = searchEntry;
                        break;
                    }
                }

                if (entry != null)
                {
                    _entryById[id] = entry;
                }
            }

            if (entry != null && entry.Data != null)
            {
                value = entry.Data;
                return true;
            }

            value = default;
            return false;
        }

        public TEntry FirstOrDefaultEntry(Func<TEntry, bool> predicate) => _entries?.FirstOrDefault(predicate);        

        public void Dispose()
        {
            _entries.Clear();
            _entryById.Clear();
        }
    }

    public abstract class Registry<TEntry> : ScriptableObject, IRegistry, IDisposable
    {
        [SerializeField]
        private List<TEntry> _entries = new();
        public List<TEntry> Entries => _entries;

        public TEntry this[int idx] => idx >= 0 && idx < _entries.Count ? Entries[idx] : default;
        public void Dispose() => _entries.Clear();

    }
}
