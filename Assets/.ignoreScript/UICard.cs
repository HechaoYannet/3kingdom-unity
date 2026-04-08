using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Card;
using System.IO;
using System;
using AssetBundleFormWork;

public class UICard : MonoBehaviour
{
    private CardDefines[] cardDefine;

    public Dictionary<int, CardDefines> cardsDic = new Dictionary<int, CardDefines>();

    public Dictionary<int, CharacterInfo> CharacterInfoDic = new Dictionary<int, CharacterInfo>();
    public static UICard instance;

    
    private void Awake()
    {
        instance = this;
        int id=1;
       
        DontDestroyOnLoad(this);
    }
   
    //保存json文件路径
    string JsonPath(CardType cardType)
    {
        string path = "";
        switch (cardType)
        {
            case CardType.CharacterCard:
                path= Application.streamingAssetsPath + "/JsonData/Character.json";
                break;
            case CardType.EquipCard:
                break;
            case CardType.Common:
                //return Application.dataPath + @"/DateMode/CardDefine.json";
                path= Application.streamingAssetsPath + "/CardDefine.json";
                break;
            default:
                break;
        }
        return path;
    }

   public void SerchCardData(CardType cardType)
    {
        ReadJson(cardType);
    }
    //从本地读取json数据
    void ReadJson(CardType cardType)
    {
        string path = JsonPath(cardType);
        if (!File.Exists(path))
        {
            Debug.LogError("读取的文件不存在！创建文件");
            return ;
        }
        string json = File.ReadAllText(path);

        Debug.Log(json);
       

        if (cardType==CardType.CharacterCard)
        {
            CharacterData characterData = JsonUtility.FromJson<CharacterData>(json);
            foreach (var item in characterData.CharacterList)
            {
                CharacterInfoDic.Add(item.cardID, item);
            }
        }
        if (cardType == CardType.Common)
        {
            CardData cardData = JsonUtility.FromJson<CardData>(json);
            //Debug.Log(cardData.cardDefineList.Count);
            foreach (var item in cardData.cardDefineList)
            {
                //Debug.Log(item.cardType);
                cardsDic.Add(item.cardID, item);
            }
        }

    }
}

public class CardData
{
    //public CardDefines[] cardDefineList;
    public List< CardDefines >cardDefineList=new List<CardDefines>();
}


public class CharacterData
{
    public  List<CharacterInfo> CharacterList = new List<CharacterInfo>();
}

[Serializable]
public class CharacterInfo
{
    public int cardID;
    public string IDName;
    public string CardName;
    public string _CharacterInfo;
    public string URL;
    
}
