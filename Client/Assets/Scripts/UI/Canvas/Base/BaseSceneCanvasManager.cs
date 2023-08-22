using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class BaseSceneCanvasManager : MonoBehaviour
{
    private Dictionary<CanvasType, BaseCanvas> _canvasDictionary = new Dictionary<CanvasType, BaseCanvas>();
    private Stack<CanvasType> _beforeCanvasStack = new Stack<CanvasType>();
    public CanvasType BeforeCanvas
    {
        get
        {
            return _beforeCanvasStack.Pop();
        }
    }

    protected CanvasType _activeCanvas;
    public CanvasType ActiveCanvas => _activeCanvas;

    private float _openDelay = 0f;
    public float OpenDelay => _openDelay;

    protected virtual void Awake()
    {
        BaseCanvas[] canvasArray = GetComponentsInChildren<BaseCanvas>(true);
        foreach (BaseCanvas canvas in canvasArray)
        {
            _canvasDictionary.Add(canvas.CanvasType, canvas);
        }
    }

    private void Start()
    {
        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Escape, InputEscape);
    }

    private void Update()
    {
        if(_openDelay > 0f)
        {
            _openDelay -= Time.deltaTime;
        }
    }

    private void InputEscape()
    {
        if (_openDelay <= 0f)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene)
            {
                if (_activeCanvas == CanvasType.Menu)
                {
                    ChangeCanvas(CanvasType.Setting, _activeCanvas);
                }
                else
                {
                    ChangeBeforeCanvas();
                }
            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene || UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TrainingScene)
            {
                if (_activeCanvas == CanvasType.Pause)
                {
                    ChangeCanvas(CanvasType.Information, _activeCanvas);
                }
                else if (_activeCanvas != CanvasType.GameOver)
                {
                    ChangeCanvas(CanvasType.Pause, _activeCanvas);
                }
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene)
            //{
            //    if (_activeCanvas != CanvasType.Setting)
            //    {
            //        ChangeCanvas(CanvasType.Setting, _activeCanvas);
            //    }
            //}
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene || UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TrainingScene)
            {
                if (_activeCanvas != CanvasType.Pause)
                {
                    if (_activeCanvas == CanvasType.GameOver || _activeCanvas == CanvasType.Setting)
                        return;

                    ChangeCanvas(CanvasType.Pause, _activeCanvas);
                }
            }
        }
    }

    public virtual void ChangeCanvas(CanvasType canvasType, CanvasType beforeCanvas = CanvasType.Base)
    {
        if(canvasType != CanvasType.Menu && beforeCanvas != CanvasType.Base)
        {
            _beforeCanvasStack.Push(beforeCanvas);
        }

        foreach (var canvas in _canvasDictionary)
        {
            canvas.Value.Canvas.enabled = canvas.Key == canvasType;
            if (canvas.Key == canvasType)
                canvas.Value.OnOpenEvents();
            else if (canvas.Key == beforeCanvas)
                canvas.Value.OnCloseEvents();
        }

        _openDelay = 0.1f;
        _activeCanvas = canvasType;
    }

    public virtual void ChangeBeforeCanvas()
    {
        if (_beforeCanvasStack.Count == 0)
            return;
        CanvasType canvasType = _beforeCanvasStack.Pop();

        foreach (var canvas in _canvasDictionary)
        {
            canvas.Value.Canvas.enabled = canvas.Key == canvasType;
            if (canvas.Key == canvasType)
                canvas.Value.OnOpenEvents();
            else if (canvas.Key == _activeCanvas)
                canvas.Value.OnCloseEvents();
        }
        
        _openDelay = 0.1f;
        _activeCanvas = canvasType;
    }

    public BaseCanvas GetCanvas(CanvasType canvasType)
    {
        if (_canvasDictionary.ContainsKey(canvasType))
        {
            return _canvasDictionary[canvasType];
        }
        return null;
    }

    public void BeforeCanvasClear()
    {
        _beforeCanvasStack.Clear();
    }

}
