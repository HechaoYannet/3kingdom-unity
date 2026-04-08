using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public static class DictionaryHelper 
{
    /// <summary>
    /// 字典排序(默认排序)
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    /// <typeparam name="TValue">Value类型</typeparam>
    /// <param name="dictionary">无序字典</param>
    /// <returns>有序字典</returns>
    public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException("字典为空,不能排序");
        }

        return new SortedDictionary<TKey, TValue>(dictionary);
    }

    /// <summary>
    /// 字典排序(自定义排序)
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    /// <typeparam name="TValue">Value类型</typeparam>
    /// <param name="dictionary">无序字典</param>
    /// <param name="comparer">自定义排序方法</param>
    /// <returns>有序字典</returns>
    public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException("字典为空,不能排序");
        }

        if (comparer == null)
        {
            throw new ArgumentNullException("自定义排序方法为空,不能排序");
        }

        return new SortedDictionary<TKey, TValue>(dictionary, comparer);
    }

    /// <summary>
    /// 字典排序(依据值默认排序)
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    /// <typeparam name="TValue">Value类型</typeparam>
    /// <param name="dictionary">无序字典</param>
    /// <returns>有序字典</returns>
    public static IDictionary<TKey, TValue> SortByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        return dictionary.OrderBy(v => v.Value).ToDictionary(item => item.Key, item => item.Value);
    }

    /// <summary>
    /// 字典排序(依据值自定义排序)
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    /// <typeparam name="TValue">Value类型</typeparam>
    /// <param name="dictionary">无序字典</param>
    /// <param name="comparer">自定义排序方法</param>
    /// <returns>有序字典</returns>
    public static IDictionary<TKey, TValue> SortByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IComparer<TValue> comparer)
    {
        return dictionary.OrderBy(v => v.Value, comparer).ToDictionary(item => item.Key, item => item.Value);
    }

    /// <summary>
    /// 判断字典是否为空或者无数据
    /// </summary>
    /// <param name="dictionary">字典</param>
    /// <returns>True:字典为空或没有数据;否则为False</returns>
    public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException("字典为空");
        }

        return dictionary.Count == 0;
    }

    /// <summary>
    /// 判断字典是否为空或者无数据
    /// </summary>
    /// <param name="dictionary">字典</param>
    /// <returns>True:字典为空或没有数据;否则为False</returns>
    public static bool IsNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        return dictionary == null || dictionary.Count == 0;
    }

    /// <summary>
    /// 获取字典值(如果不包含该值则由自定义方法返回)
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    /// <typeparam name="TValue">Value类型</typeparam>
    /// <param name="dictionary">字典</param>
    /// <param name="key">Key值</param>
    /// <param name="function">自定义方法</param>
    /// <returns>字典里的值或者自定义方法返回值</returns>
    public static TValue GetOrGetByFunction<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> function)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException("字典为空");
        }

        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }

        if (function == null)
        {
            throw new ArgumentNullException("自定义取值方法为空,不能获取值");
        }

        return function();
    }

    /// <summary>
    /// 新增或重写(如果没有该Key值,则新增;否则重写该值)
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    /// <typeparam name="TValue">Value类型</typeparam>
    /// <param name="dictionary">字典</param>
    /// <param name="key">Key值</param>
    /// <param name="value">Value新值</param>
    /// <returns>字典</returns>
    public static IDictionary<TKey, TValue> AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException("字典为空");
        }

        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        return dictionary;
    }

    /// <summary>
    /// 随机获取字典中的一个key
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static TKey RandomDictionaryKeyValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException("字典为空");
        }
        List<TKey> keyList = new List<TKey>(dictionary.Keys);
        Random rand = new Random();
        return keyList[rand.Next(keyList.Count)];
    }
}

