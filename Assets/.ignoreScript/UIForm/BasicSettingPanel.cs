using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSettingPanel : BaseUiFrame
{
    public static BasicSettingPanel _instance;
    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        RigisterButtonObjectEvent("StartGameBtn", P=> BeginGameEnvent());
        RigisterButtonObjectEvent("BackBtn", P => BackGameEnvent());
    }

    private void BackGameEnvent()
    {
      
    }
    [HideInInspector]
    public LoadingWind Loading = null;

    public void BeginGameEnvent()
    {
        //SceneManager.LoadScene("GameScene");
            UiManager.Instance().ShowUiForms(UiWind.LoadingWind.ToString());
            Loading = GameObject.Find("LoadingWind(Clone)").GetComponent<LoadingWind>();
        ResSvc.Instance().AsyncLoadScene("GameScene", () =>
        {
            UICard.instance.SerchCardData(CardType.Common);
            UiManager.Instance().ShowUiForms(UiWind.GamingPanel.ToString());
        });
    }
    
}
