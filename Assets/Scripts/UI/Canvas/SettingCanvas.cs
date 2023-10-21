using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Event;
using UnityEditor;

public class SettingCanvas : BaseCanvas
{
    [SerializeField]
    private AudioClip _sliderSound = null;
    private float _sliderSoundDelay = 0f;

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

    private void Update()
    {
        if (_sliderSoundDelay > 0f)
        {
            _sliderSoundDelay -= Time.unscaledDeltaTime;
        }
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

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region Audio
    public void OnMasterSlider(float value)
    {
        if(_sliderSoundDelay <= 0f)
        {
            PlayButtonSound(_sliderSound);
            _sliderSoundDelay = 0.1f;
        }

        SoundManager.Instance.MasterVolume = value;
        _audioMixer.SetFloat("Master", value);
    }

    public void OnBgmSlider(float value)
    {
        if (_sliderSoundDelay <= 0f)
        {
            PlayButtonSound(_sliderSound);
            _sliderSoundDelay = 0.1f;
        }

        SoundManager.Instance.BgmVolume = value;
        _audioMixer.SetFloat("BGM", value);
    }

    public void OnSfxSlider(float value)
    {
        if (_sliderSoundDelay <= 0f)
        {
            PlayButtonSound(_sliderSound);
            _sliderSoundDelay = 0.1f;
        }

        SoundManager.Instance.SfxVolume = value;
        _audioMixer.SetFloat("SFX", value);
    }
    #endregion
}
