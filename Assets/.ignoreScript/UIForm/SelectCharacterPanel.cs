using AssetBundleFormWork;
using Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterPanel : BaseUiFrame
{
    public static SelectCharacterPanel _instance;
    // 场景名称 一般是场景名称，也可以是任意的名称（只需要是二级目录）
    private string _SceneName = "card";
    // AB包名
    private string _AsserBundleName = "character.ab"; // "scene_1/prefabs_1.ab"
                                                      // 资源名称 只需要最后的名称
    private string _AssetName = "Sphere.prefab"; // “Sphere”
    private Transform Content;
    private GameObject[] CharacterCardGo;

    private Dictionary<string, CharacterCardOnPoint> characterDic = new Dictionary<string, CharacterCardOnPoint>();
    private void Awake()
    {
        _instance = this;
        CurrentUiType.UiShowMode = UiShowMode.Normal;
        CurrentUiType.UiWindType = UiWindType.Common;

        Content = UnityHelper.FindTheChildNode(gameObject,"Content");

        UICard.instance.SerchCardData(CardType.CharacterCard);
        UiManager.Instance().ShowUiForms(UiWind.CharacterInfoPop.ToString());

      

    }
    public void OpenSelectCharacterPanel()
    {
        OpenUIForm(UiWind.SelectCharacterPanel.ToString());
    }

    private void Start()
    {
        InitCharacter();
       
    }
    public void InitCharacter()
    {
        //StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundlePack("card", "character.ab", LoadAllComplete));
        StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundlePack(_SceneName, _AsserBundleName, LoadAllComplete));
        

    }

    private void LoadAllComplete(string abName)
    {
        AssetBundleManager.GetInstance().ShowAllABname();
        CharacterCardGo = new GameObject[CharacterCard.GetInstance().Length];
        for (int i = 0; i < CharacterCardGo.Length; i++)
        {
            CharacterCardGo[i] = CardManager.InitPrefab("Prefabs/CharacterUICard", Content.gameObject);
            CharacterCardGo[i].AddComponent<CharacterCardOnPoint>();
            CharacterCardOnPoint characterCardOnPoint = CharacterCardGo[i].GetComponent<CharacterCardOnPoint>();
            characterCardOnPoint.Name = Enum.GetName(typeof(CharacterName), i);
            characterDic.Add(characterCardOnPoint.Name, characterCardOnPoint);
            CharacterCardGo[i].transform.GetChild(0).GetComponent<Image>().sprite = AssetBundleManager.GetInstance().LoadAsset<Sprite>("card", "character.ab", Enum.GetName(typeof(CharacterName), i), false);
        }
        CharacterInfoPop._isntance.InitInfo("GuanYu");
    }

    string IndexName = "";
    public  void SelectCharacter(string cardName)
    {
        if (characterDic.ContainsKey(IndexName))
            characterDic[IndexName].IsSelect = false;
        IndexName = cardName;
        characterDic[IndexName].IsSelect = true;
    }
}
   
public class CharacterCard
{
    public static CharacterInfo[] CharacterCardDefines;

    public static CharacterInfo[] GetInstance()
    {
        InitCharacterCard();
        return CharacterCardDefines;
    }
    public static void InitCharacterCard()
    {
        CharacterCardDefines = new CharacterInfo[8];
        for (int i = 0; i < 8; i++)
        {
            CharacterCardDefines[i] = UICard.instance.CharacterInfoDic[1 + i];
            //Debug.LogError(CharacterCardDefines[i].CardName);
        }
    }

    public static CharacterInfo GetCardDefineByIDName(string IDName)
    {
        //Debug.LogError(IDName);
        foreach (var item in CharacterCardDefines)
        {
            if(item.IDName == IDName)
            {
                return item;
            }
        }

        return null;
    }
}


public enum CharacterName
{
    GuanYu=0,CaiWenJi=1, CaoYing=2, DingFeng=3, DongBai=4, GuanSuo=5, GongSunZan= 6, YueJin=7
}