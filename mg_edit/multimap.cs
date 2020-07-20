using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg_edit
{
    class MultiMap<Key, Value>
    {
        // Internal hashmap that is the backbone
        private Dictionary<Key, List<Value>> internalMap = new Dictionary<Key, List<Value>>();

        // Adds new element to given key
        public void Add(Key key, Value value)
        {
            if (!internalMap.ContainsKey(key))
            {
                internalMap[key] = new List<Value>();
            }
            internalMap[key].Add(value);
        }

        // Returns true if multimap contains key
        public bool Has(Key key)
        {
            return internalMap.ContainsKey(key);
        }

        // Gets reference to list at given key
        // Returns null if key does not exist
        public List<Value> Get(Key key)
        {
            if (!internalMap.ContainsKey(key))
            {
                return null;
            }
            return internalMap[key];
        }

        // Returns a list of key to value
        public List<(Key, Value)> AsList()
        {
            List<(Key, Value)> list = new List<(Key, Value)>();

            foreach (var kv in internalMap)
            {
                foreach (var v in kv.Value)
                {
                    list.Add((kv.Key, v));
                }
            }
            
            return list;
        }
    }
}
