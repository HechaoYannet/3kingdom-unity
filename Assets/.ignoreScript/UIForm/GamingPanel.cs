using AssetBundleFormWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamingPanel : BaseUiFrame
{
    public static GamingPanel _isntance;
    // Start is called before the first frame update
    void Awake()
    {
        _isntance = this;
        CurrentUiType.UiShowMode = UiShowMode.Normal;
        CurrentUiType.UiWindType = UiWindType.Common;
    }
    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            switch (i)
            {
                case 0:
                    StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundlePack("audio", "ogg.ab", LoadAllCompleteClip));
                    break;
                case 1:
                    StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundlePack("card", "game.ab", LoadAllCompleteClip));
                    break;

                default:
                    break;
            }
        }
    }
    private void LoadAllCompleteClip(string abName)
    {
        Debug.Log("******LoadAllCompleteClip*******");
        AssetBundleManager.GetInstance().ShowAllABname();
    }
    public void OpenSelectCharacterPanel()
    {
        OpenUIForm(UiWind.GamingPanel.ToString());
    }
}
