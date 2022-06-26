using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseUtil.Base
{
    public static class Fn
    {
        public static List<O> Map<I, O>(Func<I, O> fn, List<I> collection)
        {
            // same as collection.Select(FnVal.AsInt).ToList()
            List<O> list = new List<O>();
            collection.ForEach((x) => { list.Add(fn(x)); });
            return list;
        }

        public static List<T> Filter<T>(Func<T, bool> fn, List<T> collection)
        {
            // same as collection.Where(FnVal.StartsWith(idPrefix)).ToList()
            List<T> list = new List<T>();
            collection.ForEach((x) =>
            {
                if (fn(x))
                {
                    list.Add(x);
                }
            });
            return list;
        }

        /**
         * Return true ONLY if all of the fns return true
         */
        public static bool All<T>(Func<T, bool> fn, List<T> collection)
        {
            return collection.All(fn);
        }

        /**
         * Return true ONLY if any of the fns returns true
         */
        public static bool Any<T>(Func<T, bool> fn, List<T> collection)
        {
            return collection.Any(fn);
        }

        public static void EachInArray<T>(Action<T> fn, T[] collection)
        {
            foreach (T item in collection)
            {
                fn(item);
            }
        }

        public static Dictionary<TKey, TValue> ModifyDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }
            dictionary[(key)] = value;
            return dictionary;
        }

        public static Dictionary<TKey, TValue> SetDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>(dictionary);
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }
            dict[(key)] = value;
            return dict;
        }

        public static Dictionary<TKey, TValue> ListToDictionaryWithKeyFn<TKey, TValue>(Func<TValue, TKey> keyFn, List<TValue> list)
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            if (list == null) return dict;
            if (list.Count == 0) return dict;
            list.ForEach((v) =>
            {
                TKey key = keyFn(v);
                dict = Fn.SetDictionary(dict, key, v);
            });
            return dict;
        }

        public static List<T> ConcatAll<T>(List<List<T>> lists)
        {
            List<T> allItems = new List<T>();
            foreach (var list in lists)
            {
                allItems.AddRange(list);
            }
            return allItems;
        }

        public static bool DictionaryHasNullValue<K, V>(Dictionary<K, V> dictionary)
        {
            return dictionary.Any((v) => (v.Value == null));
        }

        public static bool NestedDictionaryHasNullValue<K, V>(Dictionary<K, Dictionary<K, V>> dictionary)
        {
            return dictionary.Any((x) => (x.Value.Any((v) => (v.Value == null))));
        }

        public static List<T> WithoutNull<T>(List<T> items)
        {
            return Filter(x => x != null, items);
        }

        public static void DoNothing()
        {
            // do nothing
        }

        public static void RunOnce(bool hasRun, Action<bool> setHasRunFn, Action fn)
        {
            if (!hasRun)
            {
                setHasRunFn(true);
                fn();
            }
        }

        /// <summary>
        /// Runs e.g. 3 times
        /// </summary>
        public static void Times(int times, Action<int> fn)
        {
            for (int i = 0; i < times; i++) fn(i);
        }
    }
}