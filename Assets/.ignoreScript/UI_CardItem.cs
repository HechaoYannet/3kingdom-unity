using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * 
 * 弃用
 * 
 */



public class UI_CardItem : EnventOnPoint
{
    private static Color NormalColor = Color.white;
    private static Color SelectColor = new Color(0.4f, 0.75f, 1f, 1f);

    public Image CardImage;

    public bool IsCanPoint = false;

    public CharacterType characterType;

    private bool isSelect;
    public bool IsSelect { get => isSelect;

        set
        {
            isSelect = value;
            if (isSelect)
            {
                //Debug.LogError("被点击");
                CardImage.transform.localPosition = new Vector3(0, 40, 0);
            }
            else
            {

                CardImage.transform.localPosition = Vector3.zero;
            }
        }
    }

    private int cardID;

    string path="UI/Card/";
    public void Init(CharacterType characterType,int cardID,CardDefines cardDefines)
    {
            this.characterType = characterType;
        if(characterType==CharacterType.Player)
        {
            IsCanPoint = true;
        }
        else
        {
            IsCanPoint = false;
            cardID = -1;
        }
        this.cardID = cardID;
        Debug.Log(cardDefines.cardID);
        Debug.Log(cardDefines.cardName);
        CardImage.sprite = ResourcesLoad<Sprite>.LoadRes(path + cardDefines.cardName);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if(CardManager.instance)
        { 
            CardManager.instance.SelectedCard(this.characterType,cardID);
            if(IsCanPoint==true)
               this.IsSelect = true;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        CardImage.color = SelectColor;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        CardImage.color = NormalColor;
    }
}


