using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * ÆúÓÃ
 * 
 */


public class ResourcesLoad<T> : MonoBehaviour where T:Object
{
    public static ResourcesLoad<T> instance;
    void Start()
    {
        instance = this;
    }

   public static T LoadRes(string path)
    {
        return Resources.Load<T>(path);
    }
}
