using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingCanvas : BaseCanvas
{
    [Header("Frame")]
    [SerializeField]
    private GameObject[] _frames;
    [SerializeField]
    private Text[] _frameTexts;

    [Header("BGM")]
    [SerializeField]
    private Transform _bgmSwitch;
    [SerializeField]
    private Toggle _bgmMuteToggle;

    private static bool _isBgmOn = true;

    [Header("SFX")]
    [SerializeField]
    private Transform _sfxSwitch;
    [SerializeField]
    private Toggle _sfxMuteToggle;

    private static bool _isVfxOn = true;

    private bool _isOpen = false;

    private void Awake()
    {
        _bgmMuteToggle.isOn = _isBgmOn;
        _sfxMuteToggle.isOn = _isVfxOn;
    }

    /// <summary> ESC 체크 </summary>
    private void OnGUI()
    {
        if (UnityEngine.Event.current.type == EventType.KeyDown && UnityEngine.Event.current.keyCode == KeyCode.Escape)
        {
            OpenSettingCanvas();
        }
    }

    /// <summary> 설정창을 열려고 할때 실행하는 함수 </summary>
    private void OpenSettingCanvas()
    {
        if(_isOpen == false)
        {
            if(Time.timeScale == 1f)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene)
                {
                    Time.timeScale = 0f;
                    _isOpen = true;
                }
                CanvasManager.ChangeCanvas(CanvasType.Setting);
            }
        }
        else
        {
            OnBackButton();
        }
    }

    /// <summary> 설정창 닫는 함수 </summary>
    public void OnBackButton()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene)
        {
            CanvasManager.ChangeCanvas(CanvasType.Menu);
        }
        else if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene)
        {
            Time.timeScale = 1f;
            _isOpen = false;
            CanvasManager.ChangeCanvas(CanvasType.Controller);
        }
    }

    /// <summary> 메뉴씬으로 돌아가는 함수 </summary>
    public void OnHomeButton()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene)
        {
            CanvasManager.ChangeCanvas(CanvasType.Menu);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene)
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
            Pool.PoolManager.DeleteAllPool();
        }
    }

    /// <summary> 프레임 바꾸는 함수 </summary>
    public void OnChangeFrame(int idx)
    {
        switch(idx)
        {
            case 0:
                {
                    _frames[idx].SetActive(true);
                    _frameTexts[idx].color = Color.black;
                    _frames[1].SetActive(false);
                    _frameTexts[1].color = Color.white;
                }
                break;
            case 1:
                {
                    _frames[idx].SetActive(true);
                    _frameTexts[idx].color = Color.black;
                    _frames[0].SetActive(false);
                    _frameTexts[0].color = Color.white;
                }
                break;
            default:
                break;
        }
    }

    #region Normal
    #region BGM
    /// <summary> BGM을 ON/OFF하는 함수 </summary>
    public void OnBgmMute(bool isOn)
    {
        if(isOn)
        {
            _bgmSwitch.DOLocalMoveX(-25f, 0.2f);
        }
        else
        {
            _bgmSwitch.DOLocalMoveX(25f, 0.2f);
        }
        _isBgmOn = isOn;
    }

    /// <summary> BGM 볼륨 줄이는 함수 </summary>
    public void OnBgmMinus()
    {

    }

    /// <summary> BGM 볼륨 키우는 함수 </summary>
    public void OnBgmPlus()
    {

    }
    #endregion

    #region SFX
    /// <summary> SFX을 ON/OFF하는 함수 </summary>
    public void OnSfxMute(bool isOn)
    {
        if (isOn)
        {
            _sfxSwitch.DOLocalMoveX(-25f, 0.2f);
        }
        else
        {
            _sfxSwitch.DOLocalMoveX(25f, 0.2f);
        }
        _isVfxOn = isOn;
    }

    /// <summary> SFX 볼륨 줄이는 함수 </summary>
    public void OnSfxMinus()
    {

    }

    /// <summary> VFX 볼륨 키우는 함수 </summary>
    public void OnSfxPlus()
    {

    }
    #endregion
    #endregion

    #region Graphic
    #endregion
}
