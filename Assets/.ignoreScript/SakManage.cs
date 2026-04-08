using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
///对话系统
/// </summary>
public class SakManage:MonoBehaviour
{
    Dropdown dropdown;
    Text text;
    Transform _SakContent;

    private Msg<string>[] testMsg=new Msg<string>[2];
    private void Awake()
    {
        _SakContent = UnityHelper.GetTheChildNodeComponetScripts<Transform>(gameObject, "SakContent");
        text= UnityHelper.GetTheChildNodeComponetScripts<Text>(_SakContent.gameObject, "Text");
        _SakContent.gameObject.SetActive(false);
        dropdown = UnityHelper.GetTheChildNodeComponetScripts<Dropdown>(gameObject, "Dropdown");
        dropdown.options.Clear();
        for (int i = 0; i < _SalConten.Length; i++)
        {
            AddDropDownOptionData(_SalConten[i]);
        }

        dropdown.onValueChanged.AddListener((s)=>DropDownValue(s));
    }

    Vector3 InitlocalPosition = Vector3.zero;
    private void DropDownValue(int s)
    {
        _SakContent.gameObject.SetActive(true);
        text.text = _SalConten[s];
        testMsg[0]= new Msg<string>(MessegeType.Sak.ToString(), _SalConten[s]);
        InitlocalPosition = _SakContent.position;
     Tween t=  _SakContent.DOMoveY(InitlocalPosition.y + 150.0f, 1.5f);
        t.OnComplete(() => {
            _SakContent.position = InitlocalPosition;
            _SakContent.gameObject.SetActive(false);
            //发送消息
            MsgHandler<string>.SendMsg(MessegeType.Sak.ToString(), testMsg);
        });
    }

    /// <summary>
    /// 给下拉框添加下拉数据
    /// </summary>
    /// <param name="itemText"></param>
    public void AddDropDownOptionData(string itemText)
    {
        Dropdown.OptionData data = new Dropdown.OptionData();
        data.text = itemText;
        //data.image = "";
        dropdown.options.Add(data);
    }

    string[] _SalConten = new string[] {
    "等得花都谢了",
    "干饭人，干饭魂",
     "快点儿，等得都憔悴了"
    };
}

public enum MessegeType
{
    Sak
}