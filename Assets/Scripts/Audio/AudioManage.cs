using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManage : MonoSingleton<AudioManage>
{
    public  AudioSource audioSource;
    public  AudioClip audioClip;
    // Start is called before the first frame update

    //public static AudioManage GetInstance()
    //{
    //    new GameObject("Cubue");
    //    if(Instance==null)
    //    {
    //        Debug.LogError("执行");

    //       new GameObject("_AudioManage");
    //          //Instance = new GameObject("_AudioManage").AddComponent<AudioManage>();
    //    }
    //    return Instance;
    //}

    public AudioMixer audioMixer;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    protected override void Init()
    {
        base.Init();
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioSource.GetAudioMixerGroup(3);

    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clip"></param>
    public  void PlayAudio(AudioClip clip)
    {
        audioClip = clip;
        audioSource.PlayOneShot(audioClip);
    }
    
}



