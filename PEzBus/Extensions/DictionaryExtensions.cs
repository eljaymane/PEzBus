using System.Reflection;

namespace PEzbus;

public static class DictionaryExtensions
{
    public static void AddOrUpdate(this Dictionary<MethodInfo, IEnumerable<Type>> dictionary,
        MethodInfo key, IEnumerable<Type> value)
    {
        if (dictionary.ContainsKey(key))
            dictionary[key] = dictionary[key].Concat(value);
        else dictionary.TryAdd(key, value);
    }
}