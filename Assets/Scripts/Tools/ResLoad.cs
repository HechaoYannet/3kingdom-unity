using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundleFormWork;

public class ResLoad<T>:MonoBehaviour  where T:UnityEngine.Object
{
    public static ResLoad<T> Instance ;

    public static T t;
    // 场景名称 一般是场景名称，也可以是任意的名称（只需要是二级目录）
    private static  string _SceneName ;
    // AB包名
    private static  string _AsserBundleName ; // "scene_1/prefabs_1.ab"
                                                      // 资源名称 只需要最后的名称
    private static string _AssetName ; // “Sphere”
    public void   LoadAsset(string _sceneName, string _asserBundleName,string _assetName)
    {
        _SceneName = _sceneName;
        _AsserBundleName = _asserBundleName;
        _AssetName = _assetName;
     
        // 加载 AB 包 
       
    }

    public void  GetInstace(string _sceneName, string _asserBundleName, string _assetName) 
    {
        _SceneName = _sceneName;
        _AsserBundleName = _asserBundleName;
        _AssetName = _assetName;
       
       
           StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundlePack(_SceneName, _AsserBundleName, LoadAllCompleteClip));
       
       
    }


    private void Awake()
    {
        Instance = this;
    }


    private void LoadAllCompleteClip(string abName)
    {
       t  = AssetBundleManager.GetInstance().LoadAsset<T>(_SceneName, abName, _AssetName, false) as T;
        Debug.LogError("*************");
        AssetBundleManager.GetInstance().ShowAllABname();
    }

    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="abName"></param>
    private void LoadAllComplete(string abName)
    {

        //Debug.LogError(abName);
        //T tmpObj = null;
        // 也可以采用泛型加载
        //_Type = AssetBundleManager.GetInstance().LoadAsset(_SceneName, abName, _AssetName, false) as T;
        // //= AssetBundleManager.GetInstance().LoadAsset<T>(_SceneName, abName, _AssetName, false) as T;
        //Debug.LogError("*************");
        ////Debug.LogError(_Type);
        //AssetBundleManager.GetInstance().ShowAllABname();

        //if (tmpObj != null)
        //{
        //    return tmpObj;
        //}

        // 测试泛型加载
        // 测试结果表明：从AB包中加载同一个资源时，指向的是同一个内存地址 
        //Sprite sprite1 = AssetBundleManager.GetInstance().LoadAsset<Sprite>(_SceneName, "Textures_1.ab", "Floor", false);
        //Debug.Log(sprite1.name);
        //sprite1.name = "hhhhh";
        //Sprite sprite2 = AssetBundleManager.GetInstance().LoadAsset<Sprite>(_SceneName, "Textures_1.ab", "Floor", false);
        //Debug.Log(sprite2.name);
        //// 测试说明：采用 缓存机制，可以有效的提高效率
        //Sprite sprite3 = AssetBundleManager.GetInstance().LoadAsset<Sprite>(_SceneName, "Textures_1.ab", "WhileFloor", true);
        //Debug.Log(sprite3.name);
        //sprite3.name = "wwww";
        //Sprite sprite4 = AssetBundleManager.GetInstance().LoadAsset<Sprite>(_SceneName, "Textures_1.ab", "WhileFloor", true);
        //Debug.Log(sprite4.name);


    }


}

public enum ObjectType
{
   Sprite,
   AudioClip
}