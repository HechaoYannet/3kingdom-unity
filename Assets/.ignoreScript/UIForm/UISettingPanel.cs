using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class UISettingPanel : MonoBehaviour
{
    private Button SettingBtn;

    private GameObject SetPanel;
    private bool isOnclik =false;
    public bool IsOnclik 
    {
        get { return isOnclik; }
        set
        {
            isOnclik = value;
            if (isOnclik)
                SetPanel.SetActive(true);
            else
                SetPanel.SetActive(false);
        }
    }

    private ScrollButton scrollButton;
    private bool istate = false;
    public bool IsState
    {
        get { return istate; }
        set
        {
            istate = value;
            if (istate)
            {
                VoiceManager.SetActive(true);
                mainSlider.value = 0;
            }
            else
            {
                VoiceManager.SetActive(false);
                mainSlider.value =-80;
            }
        }
    }

    private GameObject VoiceManager;

    private Slider mainSlider;
    private Slider BgSlider;
    private Slider EffectSilder;
    private Slider VideoSlider;

    private AudioMixer audioMixer;

    private Button CloseBtn;
    private Button return_btn;
    private void Awake()
    {
        SettingBtn = UnityHelper.GetTheChildNodeComponetScripts<Button>(this.gameObject, "Set_Btn");
        SettingBtn.onClick.AddListener(() => { IsOnclik = IsOnclik ? false : true; }) ;
        SetPanel = UnityHelper.FindTheChildNode(this.gameObject, "Panel").gameObject;
        SetPanel.SetActive(false);

        scrollButton = UnityHelper.GetTheChildNodeComponetScripts<ScrollButton>(this.gameObject, "ScrollButton");
        scrollButton.OnCompleted.AddListener((state) => { IsState = state?true:false; });
       
        VoiceManager = UnityHelper.FindTheChildNode(this.gameObject, "voice").gameObject;
        VoiceManager.SetActive(true);
        InitSlider();

        CloseBtn = UnityHelper.GetTheChildNodeComponetScripts<Button>(this.gameObject, "close_btn");
        CloseBtn.onClick.AddListener(() => { SetPanel.SetActive(false); });
        return_btn = UnityHelper.GetTheChildNodeComponetScripts<Button>(this.gameObject, "return_btn");
        return_btn.onClick.AddListener(() => {
            var go = GameObject.Find("Canvas(Clone)/PopUI/LoadingWind(Clone)");
            if (go==null)
            {
                UiManager.Instance().ShowUiForms(UiWind.LoadingWind.ToString());
                //Loading = GameObject.Find("LoadingWind(Clone)").GetComponent<LoadingWind>();
                go = GameObject.Find("LoadingWind(Clone)");
            }
            go.SetActive(true);
            LoadingWind Loading = null;
            Loading = GameObject.Find("LoadingWind(Clone)").GetComponent<LoadingWind>();
            Loading.Display();
            ResSvc.Instance().AsyncLoadScene("LoginScene", () =>
            {
                Debug.LogError("返回灯笼裤");
            });
        });
    }
    #region  Slider
     void InitSlider()
    {
        mainSlider = UnityHelper.GetTheChildNodeComponetScripts<Slider>(this.gameObject, "mainSlider");
        BgSlider = UnityHelper.GetTheChildNodeComponetScripts<Slider>(this.gameObject, "BgSlider");
        EffectSilder = UnityHelper.GetTheChildNodeComponetScripts<Slider>(this.gameObject, "EffectSilder");
        VideoSlider = UnityHelper.GetTheChildNodeComponetScripts<Slider>(this.gameObject, "VideoSlider");

        audioMixer = ResourcesLoad<AudioMixer>.LoadRes("Mixer/AudioMixer");
        mainSlider.onValueChanged.AddListener((voluem) => { audioMixer.SetFloat("Master",voluem); });
        BgSlider.onValueChanged.AddListener((voluem) => { audioMixer.SetFloat("Bgm", voluem); });
        EffectSilder.onValueChanged.AddListener((voluem) => { audioMixer.SetFloat("Effect", voluem); });
        VideoSlider.onValueChanged.AddListener((voluem) => {audioMixer.SetFloat("Video", voluem); });
    }
    #endregion

}
