using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingCanvas : BaseCanvas
{
    [SerializeField]
    private GameObject[] _frames;
    [SerializeField]
    private Text[] _frameTexts;

    private bool _isOpen = false;

    // Awake에서 함수들을 넣어줄까, 인스펙터상으로 집어넣을까

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
    /// <summary> BGM을 뮤트 ON/OFF하는 함수 </summary>
    public void OnBgmMute(bool isMute)
    {
        if(isMute)
        {
            Debug.Log("a");
        }
        else
        {
            Debug.Log("b");
        }
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

    #region VFX
    /// <summary> VFX을 뮤트 ON/OFF하는 함수 </summary>
    public void OnVfxMute(bool isMute)
    {
        if (isMute)
        {

        }
        else
        {

        }
    }

    /// <summary> VFX 볼륨 줄이는 함수 </summary>
    public void OnVfxMinus()
    {

    }

    /// <summary> VFX 볼륨 키우는 함수 </summary>
    public void OnVfxPlus()
    {

    }
    #endregion
    #endregion

    #region Graphic
    #endregion
}
