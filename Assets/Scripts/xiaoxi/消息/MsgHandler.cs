using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgHandler<T>
{
    public delegate void DelMsgHandle(Msg<T>[] msg);

    private static Dictionary<string, DelMsgHandle> msgDics = new Dictionary<string, DelMsgHandle>();

    /// <summary>
    /// 增加消息监听
    /// </summary>
    /// <param name="msgType"></param>
    /// <param name="delMsgHandle"></param>
    public static void AddListener(string msgType, DelMsgHandle delMsgHandle)
    {
        if (msgDics == null)
            msgDics = new Dictionary<string, DelMsgHandle>();
        if (!msgDics.ContainsKey(msgType))
            msgDics.Add(msgType, null);
        msgDics[msgType] += delMsgHandle;
    }

    public static void RemoveListener(string  msgType,DelMsgHandle delMsgHandle)
    {
        if (msgDics != null && msgDics.ContainsKey(msgType))
            msgDics[msgType] -= delMsgHandle;
    }

    public static void ClearAllListeners(string msgType,DelMsgHandle delMsgHandle)
    {
        if (msgDics != null)
            msgDics.Clear();
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msgType"></param>
    /// <param name="msg"></param>
    public static void SendMsg(string msgType,Msg<T>[] msg)
    {
        Debug.LogError("发送消息");
        DelMsgHandle delMsgHandle;
        Debug.LogError(msgDics.TryGetValue(msgType, out delMsgHandle));
        if(msgDics!=null && msgDics.TryGetValue(msgType,out delMsgHandle))
        {
            Debug.LogError("发送消息2");
            if (delMsgHandle != null)
                delMsgHandle(msg);
        }
    }
}

public class Msg<T>
{
    public string Key { get; private set; }
    public T Value { get; private set; }

    public Msg(string key,T t)
    {
        this.Key = key;
        this.Value = t;
    }
}
