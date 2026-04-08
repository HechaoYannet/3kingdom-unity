using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMsg : MonoBehaviour
{
    private string key;
    private String vale;
    private Msg<string>[] testMsg=new Msg<string>[2];
    public void Start()
    {
        key = "hello";
        vale  = "你好啊";
      string   key1 = "Hi";
        string vale2 = "Hi";

        testMsg[0] = new Msg<string>(key, vale);
        testMsg[1] = new Msg<string>(key1, vale2);
        Debug.LogError("执行1");
        MsgHandler<string>.SendMsg("Msg", testMsg);
    }

 
}


