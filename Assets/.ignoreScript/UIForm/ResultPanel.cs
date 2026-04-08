using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : BaseUiFrame
{
    public static  ResultPanel instance;
    [HideInInspector]
    public LoadingWind Loading = null;
    private void Awake()
    {
        instance = this;
        CurrentUiType.UiShowMode = UiShowMode.Normal;
        CurrentUiType.UiWindType = UiWindType.PopUp;
        RigisterButtonObjectEvent("BackBtn", p => {
            var go = GameObject.Find("LoadingWind(Clone)");
            go.SetActive(true);
            Loading = GameObject.Find("LoadingWind(Clone)").GetComponent<LoadingWind>();
            Loading.Display();
           ResSvc.Instance().AsyncLoadScene("SelectCharacter", () =>
            {
                UICard.instance.SerchCardData(CardType.Common);
                GamingPanel._isntance.Display();
                //UiManager.Instance().ShowUiForms(UiWind.GamingPanel.ToString());
            });
        });
        RigisterButtonObjectEvent("ConfirmBtn", P => { Hiding(); });
    }//Tips

    public void SetContent(string content)
    {
        SetText(UnityHelper.GetTheChildNodeComponetScripts<Text>(gameObject,"Tips"), content);
    }
}
