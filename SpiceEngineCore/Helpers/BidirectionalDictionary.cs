using System;
using System.Collections.Generic;

namespace SpiceEngineCore.Helpers
{
    public class BidirectionalDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _valueByKey = new Dictionary<TKey, TValue>();
        private Dictionary<TValue, TKey> _keyByValue = new Dictionary<TValue, TKey>();

        public void Add(TKey key, TValue value)
        {
            if (_valueByKey.ContainsKey(key)) throw new ArgumentException("Duplicate key");
            if (_keyByValue.ContainsKey(value)) throw new ArgumentException("Duplicate value");

            _valueByKey.Add(key, value);
            _keyByValue.Add(value, key);
        }

        public bool ContainsKey(TKey key) => _valueByKey.ContainsKey(key);
        public bool ContainsValue(TValue value) => _keyByValue.ContainsKey(value);

        public TKey GetKey(TValue value) => _keyByValue[value];
        public TValue GetValue(TKey key) => _valueByKey[key];

        public bool TryGetKey(TValue value, out TKey key) => _keyByValue.TryGetValue(value, out key);
        public bool TryGetValue(TKey key, out TValue value) => _valueByKey.TryGetValue(key, out value);

        public void Clear()
        {
            _valueByKey.Clear();
            _keyByValue.Clear();
        }
    }
}
