using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * 
 * 弃用
 * 
 */


public class CharacterCardOnPoint : EnventOnPoint
{
    private GameObject charactetChild;
    private bool isSelect;
    public bool IsSelect
    {
        get => isSelect;

        set
        {
            isSelect = value;
            if (isSelect)
            {
                Debug.LogError("被点击");
                charactetChild.transform.localPosition = new Vector3(0, 40, 0);
            }
            else
            {

                charactetChild.transform.localPosition = Vector3.zero;
            }
        }
    }

    private void Awake()
    {
        charactetChild = transform.GetChild(0).gameObject;
    }

     public  string Name = "";
    public override void OnPointerClick(PointerEventData eventData)
    {
        SelectCharacterPanel._instance.SelectCharacter(Name);
        CharacterInfoPop._isntance.InitInfo(Name);
        //this.IsSelect = true;
    }
}
