using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//where是泛型约束 约束T类型 只能是MonoSingleton派生类 也就是子类
/// <summary>
/// 单例设计模式
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T :MonoSingleton<T>
{
    //声明自定义类型对象
    static T instance;
    public static T Instance
    {
        set {  instance=value; }
        get
        {
            //判断instance是否为空
            if (instance == null)
            {
                //全局查找一个组件 这个类型是T 也就是自定义的类型 
                instance = FindObjectOfType<T>();
                //如果全局查找不到组件则
                if (instance == null)
                {
                    
                    //诞生一个空物体 并为这个空物体 添加组件
                    new GameObject("_"+typeof(T).Name).AddComponent<T>();
                }                  
                else instance.Init(); //如果找到的话 初始化方法
            }
            else
            {
                instance.Init(); //如果找到的话 初始化方法
            }
            //返回出去
            return instance;
        }
    }
    protected virtual void Awake()
    {
        //如果对象为空
        if(instance == null)
        {
            //Debug.LogError("执行2");
            //初始化对象
            instance = this as T;
            //调用初始化方法
            Init();
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 初始化方法供子类来去使用  定义虚方法 供子类重写
    /// </summary>
    protected virtual void Init()
    {

    }
    /*
     什么时候使用泛型：
     当多个类型功能上是一致的时候使用泛型更合适，
     就比如数组排序功能 有 int float doule.... 
     他们都有共同功能哪就是排序 这个时候泛型就来了
     */
     //好好理解上面注释 这将是理解泛型关键
}
