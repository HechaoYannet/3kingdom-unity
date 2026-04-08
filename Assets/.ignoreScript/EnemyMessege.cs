using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * 
 * 弃用
 * 
 */



public class EnemyMessege : MonoBehaviour
{
    Transform _SakContent;
    Text text;
    string[] _SalConten = new string[] {
    "没事儿，这儿有花",
    "干饭方为人上人",
     "好的好的，闪"
    };
    private void Awake()
    {
        _SakContent = this.gameObject.GetComponent<Transform>();
        text= UnityHelper.GetTheChildNodeComponetScripts<Text>(_SakContent.gameObject, "Text");
        _SakContent.gameObject.SetActive(false);
        MsgHandler<string>.AddListener(MessegeType.Sak.ToString(), (msds) =>
        {
            Debug.LogError("执行2");
            foreach (var msd in msds)
            {

                switch (msd.Key)
                {
                    case "Sak":
                        var s = "";
                        if (msd.Value == "等得花都谢了")
                            s = _SalConten[0];
                        if (msd.Value == "干饭人，干饭魂")
                            s = _SalConten[1];
                        if (msd.Value == "快点儿，等得都憔悴了")
                            s = _SalConten[2];
                        text.text = s;
                        DropDownValue();
                        Debug.LogError("执行2" + msd.Value);
                        break;
                    default:
                        break;
                }

            }
        });
    }

    Vector3 InitlocalPosition;
    private void DropDownValue()
    {
        _SakContent.gameObject.SetActive(true);
       
        InitlocalPosition = _SakContent.position;
        Tween t = _SakContent.DOMoveY(InitlocalPosition.y - 150.0f, 1.5f);
        t.OnComplete(() => {
            _SakContent.position = InitlocalPosition;
            _SakContent.gameObject.SetActive(false);
          
        });
    }
}
