using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Audio;
using System;

public class SettingCanvas : BaseCanvas
{
    [Header("Frame")]
    [SerializeField]
    private RectTransform _backGround;
    [SerializeField]
    private RectTransform _frame;
    [SerializeField]
    private TextMeshProUGUI[] _frameTexts;
    [SerializeField]
    private Toggle[] _toggles;

    [Header("Audio")]
    [SerializeField]
    private AudioMixer _audioMixer;

    [Serializable]
    private struct Bgm
    {
        public Transform _bgmSwitch;
        public Toggle _bgmMuteToggle;
        public Slider _bgmSlider;
        [HideInInspector]
        public bool _isBgmMinusDown;
        [HideInInspector]
        public bool _isBgmPlusDown;

        public static bool _isBgmOn = true;
    }

    [SerializeField]
    private Bgm _bgm;

    [Serializable]
    private struct Sfx
    {
        public Transform _sfxSwitch;
        public Toggle _sfxMuteToggle;
        public Slider _sfxSlider;
        [HideInInspector]
        public bool _isSfxMinusDown;
        [HideInInspector]
        public bool _isSfxPlusDown;

        public static bool _isSfxOn = true;
    }

    [SerializeField]
    private Sfx _sfx;

    private Sequence _changeFrameSequence;

    //private void Start()
    //{
    //    _bgm._bgmSlider.value = SoundManager.Instance.BgmVolume;
    //    _sfx._sfxSlider.value = SoundManager.Instance.SfxVolume;

    //    _bgm._bgmMuteToggle.isOn = Bgm._isBgmOn;
    //    _sfx._sfxMuteToggle.isOn = Sfx._isSfxOn;
    //}

    //public override void OnOpenEvents()
    //{
    //    base.OnOpenEvents();

    //    _startSequence = DOTween.Sequence()
    //    .PrependCallback(() =>
    //    {
    //        _backGround.anchoredPosition += Vector2.down * 500f;
    //    })
    //    .Append(_backGround.DOAnchorPosY(0f, 0.5f));
    //}

    //private void Update()
    //{
    //    if (_bgm._isBgmMinusDown)
    //    {
    //        _bgm._bgmSlider.value -= 0.2f;
    //    }
    //    if (_bgm._isBgmPlusDown)
    //    {
    //        _bgm._bgmSlider.value += 0.2f;
    //    }
    //    if (_sfx._isSfxMinusDown)
    //    {
    //        _sfx._sfxSlider.value -= 0.2f;
    //    }
    //    if (_sfx._isSfxPlusDown)
    //    {
    //        _sfx._sfxSlider.value += 0.2f;
    //    }
    //}

    ///// <summary> 설정창 닫는 함수 </summary>
    //public override void OnBackButton()
    //{
    //    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene)
    //    {
    //        base.OnBackButton();
    //    }
    //    else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene || UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TrainingScene)
    //    {
    //        CanvasManager.ChangeCanvas(CanvasType.Pause, CanvasType);
    //    }
    //    if (_toggles[1].isOn == true)
    //    {
    //        OnChangeFrame(0);
    //    }
    //}

    /// <summary> 프레임 바꾸는 함수 </summary>
    public void OnChangeFrame(int idx)
    {
        if (_toggles[idx].isOn == true) return;
        switch (idx)
        {
            case 0:
                {
                    _toggles[idx].isOn = true;
                    _frameTexts[idx].color = Color.black;

                    _changeFrameSequence.Kill();
                    _changeFrameSequence = DOTween.Sequence()
                    .Append(_frame.DOAnchorPosY(250f, 0.7f));

                    _toggles[1].isOn = false;
                    _frameTexts[1].color = Color.white;
                }
                break;
            case 1:
                {
                    _toggles[idx].isOn = true;
                    _frameTexts[idx].color = Color.black;

                    _changeFrameSequence.Kill();
                    _changeFrameSequence = DOTween.Sequence()
                    .Append(_frame.DOAnchorPosY(770f, 0.7f));

                    _toggles[0].isOn = false;
                    _frameTexts[0].color = Color.white;
                }
                break;
            default:
                break;
        }
    }

    #region Normal
    #region Audio
    #region BGM
    /// <summary> BGM을 ON/OFF하는 함수 </summary>
    public void OnBgmMute(bool isOn)
    {
        if (isOn)
        {
            _bgm._bgmSwitch.DOLocalMoveX(-25f, 0.2f);
            if (SoundManager.Instance.BgmVolume == -20f)
            {
                _bgm._bgmSlider.value = 0f;
            }
            _audioMixer.SetFloat("BGM", SoundManager.Instance.BgmVolume);
        }
        else
        {
            _bgm._bgmSwitch.DOLocalMoveX(25f, 0.2f);
            _audioMixer.SetFloat("BGM", -80f);
        }
        Bgm._isBgmOn = isOn;
    }

    public void OnBgmSlider(float value)
    {
        SoundManager.Instance.BgmVolume = value;
        _audioMixer.SetFloat("BGM", value);
        if (value <= -20f)
        {
            _bgm._bgmMuteToggle.isOn = false;
        }
        else if (Bgm._isBgmOn == false)
        {
            _bgm._bgmMuteToggle.isOn = true;
        }
    }

    /// <summary> BGM 볼륨 줄이는 함수 </summary>
    public void OnBgmMinusDown()
    {
        _bgm._isBgmMinusDown = true;
    }
    public void OnBgmMinusUp()
    {
        _bgm._isBgmMinusDown = false;
    }

    /// <summary> BGM 볼륨 키우는 함수 </summary>
    public void OnBgmPlusDown()
    {
        _bgm._isBgmPlusDown = true;
    }
    public void OnBgmPlusUp()
    {
        _bgm._isBgmPlusDown = false;
    }
    #endregion

    #region SFX
    /// <summary> SFX을 ON/OFF하는 함수 </summary>
    public void OnSfxMute(bool isOn)
    {
        if (isOn)
        {
            _sfx._sfxSwitch.DOLocalMoveX(-25f, 0.2f);
            if (SoundManager.Instance.SfxVolume == -20f)
            {
                _sfx._sfxSlider.value = 0f;
            }
            _audioMixer.SetFloat("SFX", SoundManager.Instance.SfxVolume);
        }
        else
        {
            _sfx._sfxSwitch.DOLocalMoveX(25f, 0.2f);
            _audioMixer.SetFloat("SFX", -80f);
        }
        Sfx._isSfxOn = isOn;
    }

    public void OnSfxSlider(float value)
    {
        SoundManager.Instance.SfxVolume = value;
        _audioMixer.SetFloat("SFX", value);
        if (value <= -20f)
        {
            _sfx._sfxMuteToggle.isOn = false;
        }
        else if (Sfx._isSfxOn == false)
        {
            _sfx._sfxMuteToggle.isOn = true;
        }
    }

    /// <summary> SFX 볼륨 줄이는 함수 </summary>
    public void OnSfxMinusDown()
    {
        _sfx._isSfxMinusDown = true;
    }
    public void OnSfxMinusUp()
    {
        _sfx._isSfxMinusDown = false;
    }

    /// <summary> SFX 볼륨 키우는 함수 </summary>
    public void OnSfxPlusDown()
    {
        _sfx._isSfxPlusDown = true;
    }
    public void OnSfxPlusUp()
    {
        _sfx._isSfxPlusDown = false;
    }
    #endregion
    #endregion
    #endregion

    #region Graphic
    #endregion
}
