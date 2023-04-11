using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class BaseCanvas : MonoBehaviour
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

    /// <summary> 메뉴씬으로 돌아가는 함수 </summary>
    public virtual void OnHomeButton()
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

    public virtual void OnOpenAnimation()
    {

    }
}
