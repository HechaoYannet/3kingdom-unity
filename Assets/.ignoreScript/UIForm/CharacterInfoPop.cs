using AssetBundleFormWork;
using Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Video;

public class CharacterInfoPop : BaseUiFrame
{
    public static  CharacterInfoPop _isntance;
    private void Awake()
    {
        _isntance = this;
        CurrentUiType.UiShowMode = UiShowMode.HideOther;
        CurrentUiType.UiWindType = UiWindType.PopUp;
        //Display();
    }
    
    public void OpenSelectCharacterPanel()
    {
        Debug.LogError("OpenSelectCharacterPanel:"+ "CharacterInfoPop");
        OpenUIForm(UiWind.CharacterInfoPop.ToString());
    }

    Image Icon;
    Text Name_tx;
    Text Content_tx;
    VideoPlayer videoPlayer;
    AudioSource audioSource;
    public void  InitInfo(string IconName)
    {
        Icon = UnityHelper.FindTheChildNode(gameObject, "Icon").GetComponent<Image>();
        Name_tx= UnityHelper.FindTheChildNode(gameObject, "name").GetComponent<Text>();
        Content_tx = UnityHelper.FindTheChildNode(gameObject, "content_tx").GetComponent<Text>();
        Icon.sprite = AssetBundleManager.GetInstance().LoadAsset<Sprite>("card", "character.ab", IconName, false);
        videoPlayer= UnityHelper.FindTheChildNode(gameObject, "CharacterVideo").GetComponent<VideoPlayer>();
        CharacterInfo characterInfo =  CharacterCard.GetCardDefineByIDName(IconName);
        CharacterMesseges.characterInfo = characterInfo;
        CharacterMesseges.sprite = Icon.sprite;
        Name_tx.text = characterInfo.CardName;
        Content_tx.text = characterInfo._CharacterInfo;
        videoPlayer.url = characterInfo.URL;
        //audioSource.outputAudioMixerGroup = audioSource.GetAudioMixerGroup(2);
    }
}
