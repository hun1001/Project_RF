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

    protected virtual void Awake()
    {
        BaseCanvas[] canvasArray = GetComponentsInChildren<BaseCanvas>(true);
        foreach (BaseCanvas canvas in canvasArray)
        {
            _canvasDictionary.Add(canvas.CanvasType, canvas);
        }
    }

    public virtual void ChangeCanvas(CanvasType canvasType, CanvasType beforeCanvas)
    {
        if(canvasType != CanvasType.Menu && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene)
        {
            _beforeCanvasStack.Push(beforeCanvas);
        }

        _activeCanvas = canvasType;

        foreach (var canvas in _canvasDictionary)
        {
            canvas.Value.Canvas.enabled = canvas.Key == canvasType;
            if (canvas.Key == canvasType)
                canvas.Value.OnOpenAnimation();
        }
    }

    public void ChangeBeforeCanvas()
    {
        CanvasType canvasType = _beforeCanvasStack.Pop();

        _activeCanvas = canvasType;

        foreach (var canvas in _canvasDictionary)
        {
            canvas.Value.Canvas.enabled = canvas.Key == canvasType;
            if (canvas.Key == canvasType)
                canvas.Value.OnOpenAnimation();
        }
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
