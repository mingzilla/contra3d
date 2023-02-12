using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public static O[] MapArray<I, O>(Func<I, O> fn, I[] collection)
        {
            O[] items = new O[collection.Length];
            for (int i = 0; i < collection.Length; i++)
            {
                I x = collection[i];
                items[i] = fn(x);
            }
            return items;
        }

        public static O[] MapArrayWithIndex<I, O>(Func<I, int, O> fn, I[] collection)
        {
            O[] items = new O[collection.Length];
            for (int i = 0; i < collection.Length; i++)
            {
                I x = collection[i];
                items[i] = fn(x, i);
            }
            return items;
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

        public static Dictionary<TKey, List<TValue>> GroupBy<TKey, TValue>(Func<TValue, TKey> keyFn, IEnumerable<TValue> list)
        {
            return list.GroupBy(keyFn).ToDictionary(entry => entry.Key, entry => entry.ToList());
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

        public static void DoNothing1<T>(T t)
        {
            // (params string[] values) is called params array, which doesn't work when using generics, so (params T[] ts) is not working
        }

        public static void RunOnce(bool hasRun, Action<bool> setHasRunFn, Action fn)
        {
            if (!hasRun)
            {
                setHasRunFn(true);
                fn();
            }
        }

        public static void SafeRun(Action fn)
        {
            try
            {
                fn();
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        /// <summary>
        /// Runs e.g. 3 times
        /// </summary>
        public static void Times(int times, Action<int> fn)
        {
            for (int i = 0; i < times; i++) fn(i);
        }

        public static FieldInfo[] Props(object obj)
        {
            if (obj == null) return new FieldInfo[] { };
            return obj.GetType().GetFields();
        }

        public static string[] PropNames(object obj)
        {
            return Props(obj).Select(field => field.Name).ToArray();
        }

        public static bool AreEqualObjs(object obj1, object obj2)
        {
            if (AllNulls(new[] {obj1, obj2})) return true;
            if (SomeNulls(new[] {obj1, obj2})) return false;

            Type type1 = obj1.GetType();
            Type type2 = obj2.GetType();
            if (type1 != type2) return false;

            FieldInfo[] props1 = type1.GetFields();
            FieldInfo[] props2 = type1.GetFields();

            for (int i = 0; i < props1.Length; i++)
            {
                object v1 = props1[i].GetValue(obj1);
                object v2 = props2[i].GetValue(obj2);

                if (!v1.Equals(v2)) return false;
            }
            return true;
        }

        public static bool AreEqualDictionaries(Dictionary<string, object> obj1, Dictionary<string, object> obj2)
        {
            if (AllNulls(new[] {obj1, obj2})) return true;
            if (SomeNulls(new[] {obj1, obj2})) return false;

            Dictionary<string, object>.KeyCollection keys1 = obj1.Keys;
            Dictionary<string, object>.KeyCollection keys2 = obj2.Keys;

            if (keys1.Count != keys2.Count) return false;

            foreach (string key in keys1)
            {
                object v1 = obj1[key];
                object v2 = obj2[key];

                if (AllNulls(new[] {v1, v2})) return true;
                if (SomeNulls(new[] {v1, v2})) return true;
                if (!v1.Equals(v2)) return false;
            }

            return true;
        }

        public static bool AllNulls(object[] items)
        {
            int count = items.Count(x => x is null);
            return count.Equals(items.Length);
        }

        public static bool SomeNulls(object[] items)
        {
            int count = items.Count(x => x is null);
            return !count.Equals(items.Length) && !count.Equals(0);
        }
    }
}