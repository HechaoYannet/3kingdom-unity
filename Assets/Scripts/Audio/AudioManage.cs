using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 音频管理器，负责 BGM 和战斗音效播放。
/// </summary>
public class AudioManage : MonoSingleton<AudioManage>
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioMixer audioMixer;

    private AudioSource sfxSource;
    private readonly Dictionary<BattleSfx, AudioClip> sfxClips = new Dictionary<BattleSfx, AudioClip>();

    /// <summary>
    /// 战斗音效枚举。
    /// </summary>
    public enum BattleSfx
    {
        CardDraw,
        CardPlay,
        CardHover,
        Damage,
        Heal,
        TurnStart,
        TurnEnd,
        Victory,
        Defeat,
        ButtonClick
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    protected override void Init()
    {
        base.Init();
        audioSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
    }

    /// <summary>
    /// 注册战斗音效。
    /// </summary>
    /// <param name="sfx">音效类型。</param>
    /// <param name="clip">音频片段。</param>
    public void RegisterBattleSfx(BattleSfx sfx, AudioClip clip)
    {
        sfxClips[sfx] = clip;
    }

    /// <summary>
    /// 播放战斗音效。
    /// </summary>
    /// <param name="sfx">音效类型。</param>
    public void PlayBattleSfx(BattleSfx sfx)
    {
        if (sfxClips.TryGetValue(sfx, out AudioClip clip) && clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// 播放音效。
    /// </summary>
    /// <param name="clip">音频片段。</param>
    public void PlayAudio(AudioClip clip)
    {
        audioClip = clip;
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
