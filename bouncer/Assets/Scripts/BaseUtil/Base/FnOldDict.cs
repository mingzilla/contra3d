using System;
using System.Collections.Generic;

namespace BaseUtil.Base
{
    public class FnOldDict<TKey, TValue>
    {
        private Dictionary<TKey, TValue> item = new Dictionary<TKey, TValue>();

        public FnOldDict() { }

        public static FnOldDict<TKey, TValue> Create()
        {
            return new FnOldDict<TKey, TValue>();
        }

        public FnOldDict<TKey, TValue> Remove(TKey value)
        {
            this.item.Remove(value);
            return this;
        }

        public FnOldDict<TKey, TValue> SetItem(TKey key, TValue value)
        {
            if (this.ContainsKey(key))
            {
                this.Remove(key);
            }
            this.item[(key)] = value;
            return this;
        }

        public bool ContainsKey(TKey key)
        {
            return this.item.ContainsKey(key);
        }

        public bool IsEmpty()
        {
            return this.GetCount() == 0;
        }

        public List<TKey> GetKeys()
        {
            return new List<TKey>(this.item.Keys);
        }

        public int GetCount()
        {
            return this.item.Count;
        }
    }
}