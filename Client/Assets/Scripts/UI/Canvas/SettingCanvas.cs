using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingCanvas : BaseCanvas
{
    [Header("Audio")]
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private Slider _masterSlider;
    [SerializeField]
    private Slider _bgmSlider;
    [SerializeField]
    private Slider _sfxSlider;

    private void Start()
    {
        _masterSlider.value = SoundManager.Instance.MasterVolume;
        _bgmSlider.value = SoundManager.Instance.BgmVolume;
        _sfxSlider.value = SoundManager.Instance.SfxVolume;
    }

    #region Audio
    public void OnMasterSlider(float value)
    {
        SoundManager.Instance.MasterVolume = value;
        _audioMixer.SetFloat("Master", value);
    }

    public void OnBgmSlider(float value)
    {
        SoundManager.Instance.BgmVolume = value;
        _audioMixer.SetFloat("BGM", value);
    }

    public void OnSfxSlider(float value)
    {
        SoundManager.Instance.SfxVolume = value;
        _audioMixer.SetFloat("SFX", value);
    }
    #endregion
}
