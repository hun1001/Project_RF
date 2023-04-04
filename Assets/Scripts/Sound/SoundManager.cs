using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using UnityEngine.Audio;

public enum AudioMixerType
{
    Master,
    Bgm,
    Sfx,
}

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    private AudioMixer _audioMixer;

    [HideInInspector]
    public float BgmVolume = 0f;
    [HideInInspector]
    public float SfxVolume = 0f;

    private void Awake()
    {
        BgmVolume = PlayerPrefs.GetFloat("BGM");
        SfxVolume = PlayerPrefs.GetFloat("SFX");
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("BGM", BgmVolume);
        PlayerPrefs.SetFloat("SFX", SfxVolume);
    }

    public AudioMixerGroup GetAudioMixerGroup(AudioMixerType type) => type switch
    {
        AudioMixerType.Master => _audioMixer.FindMatchingGroups("Master")[0],
        AudioMixerType.Bgm => _audioMixer.FindMatchingGroups("BGM")[0],
        AudioMixerType.Sfx => _audioMixer.FindMatchingGroups("SFX")[0],
        _ => null,
    };
}
