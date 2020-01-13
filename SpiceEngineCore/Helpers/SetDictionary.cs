using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiceEngineCore.Helpers
{
    public class SetDictionary<TKey, TValue>
    {
        private Dictionary<TKey, List<TValue>> _valueSetByKey = new Dictionary<TKey, List<TValue>>();

        public void Add(TKey key, TValue value)
        {
            if (!_valueSetByKey.ContainsKey(key))
            {
                _valueSetByKey.Add(key, new List<TValue>());
            }

            _valueSetByKey[key].Add(value);
        }

        public void AddRange(TKey key, IEnumerable<TValue> values)
        {
            if (!_valueSetByKey.ContainsKey(key))
            {
                _valueSetByKey.Add(key, new List<TValue>());
            }

            _valueSetByKey[key].AddRange(values);
        }

        public bool Remove(TKey key) => _valueSetByKey.Remove(key);
        public bool Remove(TKey key, TValue value) => _valueSetByKey[key].Remove(value);

        public bool ContainsKey(TKey key) => _valueSetByKey.ContainsKey(key);
        public bool ContainsValue(TKey key, TValue value) => _valueSetByKey.ContainsKey(key) && _valueSetByKey[key].Contains(value);

        public IEnumerable<TValue> GetValues(TKey key) => _valueSetByKey[key];

        public bool TryGetValues(TKey key, out IEnumerable<TValue> values)
        {
            if (ContainsKey(key))
            {
                values = _valueSetByKey[key];
                return true;
            }
            else
            {
                values = Enumerable.Empty<TValue>();
                return false;
            }
        }

        public void Clear(TKey key) => _valueSetByKey[key].Clear();
        public void Clear() => _valueSetByKey.Clear();
    }
}
