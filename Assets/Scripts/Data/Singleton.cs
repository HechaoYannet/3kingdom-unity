using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public  class Singleton<T> : MonoBehaviour where T : Singleton<T>
//{
//    private static volatile T instance;
//    private static object syncRoot = new Object();
//    public static T Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                lock (syncRoot)
//                {
//                    if (instance == null)
//                    {
//                        T[] instances = FindObjectsOfType<T>();
//                        if (instances != null)
//                        {
//                            for (var i = 0; i < instances.Length; i++)
//                            {
//                                Destroy(instances[i].gameObject);
//                            }
//                        }
//                        GameObject go = new GameObject();
//                        go.name = typeof(T).Name;
//                        instance = go.AddComponent<T>();
//                        DontDestroyOnLoad(go);
//                    }
//                }
//            }
//            return instance;
//        }
//    }
//}



public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>,new()
{
    private static T instance = null;

    private static readonly object locker = new object();

    public static T Instance
    {
        get
        {
            lock (locker)
            {
                if (instance == null)
                    instance = new T();
                return instance;
            }
        }
    }
}

