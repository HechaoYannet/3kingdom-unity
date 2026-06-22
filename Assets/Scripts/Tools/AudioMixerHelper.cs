using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 扩展方法
/// </summary>
public static  class AudioMixerHelper 
{
    public static AudioMixerGroup GetAudioMixerGroup(this AudioMixer audioMixer,string parent,int index)
    {
        AudioMixerGroup[] audioMixerGroup=audioMixer.FindMatchingGroups(parent);

        return audioMixerGroup[index];
    }
    public static AudioMixerGroup GetAudioMixerGroup(this AudioMixer audioMixer, int index)
    {
        AudioMixerGroup[] audioMixerGroup = audioMixer.FindMatchingGroups("Master");

        return audioMixerGroup[index];
    }
    public static AudioMixerGroup GetAudioMixerGroup(this AudioSource audioSource, int index)
    {
        AudioMixerGroup[] audioMixerGroup = Resources.Load<AudioMixer>("Mixer/AudioMixer").FindMatchingGroups("Master");

        return audioMixerGroup[index];
    }
}
