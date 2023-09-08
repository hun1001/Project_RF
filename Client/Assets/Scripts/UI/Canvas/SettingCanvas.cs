using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Event;

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

    [Header("Control")]
    [SerializeField]
    private Toggle _simpleToggle = null;
    [SerializeField]
    private Toggle _detailToggle = null;

    private int _controlType = 0;

    private void Awake()
    {
        _controlType = PlayerPrefs.GetInt("ControlType", 0);
        if (_controlType == 0)
        {
            _simpleToggle.isOn = true;
        }
        else
        {
            _detailToggle.isOn = true;
        }
    }

    private void Start()
    {
        _masterSlider.value = SoundManager.Instance.MasterVolume;
        _bgmSlider.value = SoundManager.Instance.BgmVolume;
        _sfxSlider.value = SoundManager.Instance.SfxVolume;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("ControlType", _controlType);
    }

    public void ChangePlayerControlType(int controlType)
    {
        _controlType = controlType;
        if (controlType == 0)
        {
            _simpleToggle.isOn = true;
        }
        else
        {
            _detailToggle.isOn = true;
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene || UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TutorialScene)
        {
            EventManager.TriggerEvent(EventKeyword.ChangeControlType, _controlType);
        }
    }

    public void OnClickResetButton()
    {
        PlayerPrefs.DeleteAll();
        SaveManager.DeleteAllSaveData();
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
