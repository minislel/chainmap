using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ChainMapLib
{
    public struct ChainMap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> mainDictionary = new Dictionary<TKey, TValue>();
        private List<Dictionary<TKey, TValue>> dictionaries = new List<Dictionary<TKey, TValue>>();
        #region indexer this[]
        public TValue this[TKey key]
        {
            get
            {
                if (mainDictionary.ContainsKey(key))
                {
                    return mainDictionary[key];
                }
                foreach (var d in dictionaries)
                {
                    if (d.ContainsKey(key))
                    {
                        return d[key];
                    }
                }
                throw new KeyNotFoundException($"No value found for {key}");
            }
            set
            {
                bool found = false;
                if (mainDictionary.ContainsKey(key))
                {
                    mainDictionary[key] = value;
                    found = true;
                }
                foreach (var d in dictionaries)
                {
                    if (d.ContainsKey(key))
                    {
                        mainDictionary.Add(key, value);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new KeyNotFoundException($"No value found for {key}");
                }

            }
        }
        #endregion
        #region IDictionary
        public ICollection<TKey> Keys
        {
            get
            {
                var result = new List<TKey>();
                foreach (var k in mainDictionary)
                {
                    result.Add(k.Key);
                }
                foreach (var d in dictionaries)
                {
                    foreach (var k in d)
                    {
                        result.Add(k.Key);
                    }
                }
                return result;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                var result = new List<TValue>();
                foreach (var k in mainDictionary)
                { result.Add(k.Value); }
                foreach (var d in dictionaries)
                {
                    foreach (var v in d)
                    {
                        result.Add(v.Value);
                    }
                }
                return result;
            }
        }

        public int Count
        {
            get
            {
                int result = 0;
                foreach (var k in mainDictionary)
                {
                    result++;
                }
                foreach (var d in dictionaries)
                {
                    foreach (var v in d)
                    {
                        result++;
                    }
                }
                return result;
            }
        }
        

        public ChainMap(params Dictionary<TKey, TValue>[] values)
        {
            foreach (var v in values)
            {
                dictionaries.Add(v);
            }
        }

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            if (mainDictionary.ContainsKey(key))
            { throw new ArgumentException("the specified value already exists in the main (override) dictionary!"); }
            mainDictionary.Add(key, value);
        }

        public bool TryAdd(TKey key, TValue value)
        {
            try
            {
                Add(key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Clear()
        {
            mainDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (mainDictionary.Contains(item))
            { return true; }
            foreach (var d in dictionaries)
            {
                if (d.Contains(item))
                { return true; }
            }
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            if (mainDictionary.ContainsKey(key))
            { return true; }
            foreach (var d in dictionaries)
            {

                if (d.ContainsKey(key))
                { return true; }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            List<Dictionary<TKey, TValue>> all = new List<Dictionary<TKey, TValue>>();
            all.Add(mainDictionary);
            foreach (var d in dictionaries)
            {
                all.Add(d);
            }
            foreach (var e in all)
            {
                foreach (var v in e)
                {
                    yield return new KeyValuePair<TKey, TValue>(v.Key, v.Value);
                }
            }
        }

        public bool Remove(TKey key)
        {
            if (!mainDictionary.ContainsKey(key))
            {
                return false;
            }
            else
            {
                mainDictionary.Remove(key);
                return true;
            }
        }


        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (mainDictionary.TryGetValue(key, out value) == true)
            {
                return true;
            }
            foreach (var d in dictionaries)
            {
                if (d.TryGetValue(key, out value))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ContainsValue(TValue value)
        {
            if (mainDictionary.ContainsValue(value))
            { return true; }
            foreach (var d in dictionaries)
            {
                if (d.ContainsValue(value))
                { return true; }
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            mainDictionary[item.Key] = item.Value;
        }
        #endregion
        #region ChainMap-Oriented methods
        public void AddDictionary(Dictionary<TKey, TValue> dictionary, int priority)
        {
            dictionaries.Insert(priority, dictionary);
        }
        public void RemoveDictionary(int index)
        { dictionaries.RemoveAt(index); }
        public void ClearDictionaries()
        {
            dictionaries.Clear();
            mainDictionary.Clear();
        }
        public int CountDictionaries() { return dictionaries.Count; }
        public List<Dictionary<TKey, TValue>> GetDictionaries()
        {
            List<Dictionary<TKey, TValue>> result = new List<Dictionary<TKey, TValue>>();
            result.Add(mainDictionary);
            foreach (var d in dictionaries)
            {
                result.Add(d);
            }
            return result;
        }
        public Dictionary<TKey, TValue> GetDictionary(int index)
        {
return dictionaries[index]; 
        }
        public Dictionary<TKey, TValue> GetMainDictionary()
        {
            return mainDictionary;
        }
        public Dictionary<TKey, TValue> Merge()
        {
            var result = new Dictionary<TKey, TValue>();
            foreach (var v in mainDictionary)
            {
                result.Add(v.Key, v.Value);
            }
            foreach (var d in dictionaries)
            {
                foreach (var v in d)
                {
                    if (!result.ContainsKey(v.Key))
                    {
                        result.Add(v.Key, v.Value);
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
