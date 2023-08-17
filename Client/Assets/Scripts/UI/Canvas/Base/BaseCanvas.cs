using UnityEngine;
using DG.Tweening;
using Event;
using Pool;

[DisallowMultipleComponent]
public abstract class BaseCanvas : MonoBehaviour, IButtonSound
{
    private BaseSceneCanvasManager _canvasManager = null;
    protected BaseSceneCanvasManager CanvasManager
    {
        get
        {
            if (_canvasManager == null)
            {
                _canvasManager = GetComponentInParent<BaseSceneCanvasManager>();
            }
            return _canvasManager;
        }
    }

    [SerializeField]
    private CanvasType _canvasType = CanvasType.Base;
    public CanvasType CanvasType => _canvasType;

    [SerializeField]
    protected AudioClip _buttonSound = null;

    private Canvas _canvas = null;
    public Canvas Canvas
    {
        get
        {
            if (_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
            }
            return _canvas;
        }
    }

    protected bool _isOpen = false;
    protected Sequence _startSequence;

    /// <summary> 메뉴씬으로 돌아가는 함수 </summary>
    public virtual void OnHomeButton()
    {
        PlayButtonSound();

        CanvasManager.BeforeCanvasClear();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene)
        {
            CanvasManager.ChangeCanvas(CanvasType.Menu, _canvasType);
        }
        else
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
            EventManager.ClearEvent();
            Pool.PoolManager.DeleteAllPool();
            MouseManager.Instance.ClearMouseButtonAction();
            KeyboardManager.Instance.ClearKeyActions();
        }
    }

    public virtual void OnBackButton()
    {
        CanvasManager.ChangeBeforeCanvas();

        PlayButtonSound();
    }

    public virtual void OnOpenEvents()
    {
        _isOpen = true;
    }

    public virtual void OnCloseEvents()
    {
        _isOpen = false;
    }

    public void PlayButtonSound()
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", Vector3.zero, Quaternion.identity);
        audioSource.SetSound(_buttonSound);
        audioSource.SetGroup(AudioMixerType.Sfx);
        audioSource.SetVolume(1f);
        audioSource.Play();
    }
}
