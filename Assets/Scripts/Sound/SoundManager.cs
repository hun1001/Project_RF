using UnityEngine;
using Util;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    public float MasterVolume = 0f;
    [HideInInspector]
    public float BgmVolume = 0f;
    [HideInInspector]
    public float SfxVolume = 0f;

    private void Awake()
    {
        MasterVolume = PlayerPrefs.GetFloat("Master");
        BgmVolume = PlayerPrefs.GetFloat("BGM");
        SfxVolume = PlayerPrefs.GetFloat("SFX");

        SceneManager.sceneUnloaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        VolumeSave();
    }

    private void OnSceneLoad(Scene scene)
    {
        VolumeSave();
    }

    private void VolumeSave()
    {
        PlayerPrefs.SetFloat("Master", MasterVolume);
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
