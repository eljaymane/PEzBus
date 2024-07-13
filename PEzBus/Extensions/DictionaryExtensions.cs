using System.Collections.Concurrent;
using System.Reflection;

namespace PEzbus;

public static class DictionaryExtensions
{
    public static void AddOrUpdate<TKey,TValue>(this Dictionary<TKey, IEnumerable<TValue>> dictionary,
        TKey key,TValue value)
    {
        if (dictionary.ContainsKey(key))
            dictionary[key] = dictionary[key].Concat([value]);
        else dictionary.TryAdd(key, [value]);
    }

    public static void RemoveAll<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary,
        Func<KeyValuePair<TKey, TValue>, bool> removeIf)
    {
        foreach (var item in dictionary.Where(removeIf).ToList())
        {
            dictionary.Remove(item.Key, out var value);
        }
    }
}