using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ha : MonoBehaviour
{
    public void Awake()
    {
        MsgHandler<string>.AddListener("Msg", (msds) =>
        {
        Debug.LogError("执行2");
            foreach (var msd in msds)
            {

            switch (msd.Key)
            {
                case "hello":
                    Debug.LogError("执行2" + msd.Value);
                    break;
                default:
                    break;
            }

            }
        });
    }

    //private void OnDestroy()
    //{
    //    //在脚本被销毁之前，要清除这个监听器
    //    MsgHandler<string>.RemoveListener("Msg", (msd) =>
    //    {
    //        Debug.LogError("执行3" + msd.Value);
    //        //switch (msd.Key)
    //        //{
    //        //    case "Msg":
    //        //        Debug.LogError(msd.Value);
    //        //        break;
    //        //}
    //    });
    //}
}
