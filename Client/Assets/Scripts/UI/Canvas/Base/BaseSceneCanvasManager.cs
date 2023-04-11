using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class BaseSceneCanvasManager : MonoBehaviour
{
    private Dictionary<CanvasType, BaseCanvas> _canvasDictionary = new Dictionary<CanvasType, BaseCanvas>();

    private void Awake()
    {
        BaseCanvas[] canvasArray = GetComponentsInChildren<BaseCanvas>(true);
        foreach (BaseCanvas canvas in canvasArray)
        {
            _canvasDictionary.Add(canvas.CanvasType, canvas);
        }
    }

    public virtual void ChangeCanvas(CanvasType canvasType)
    {
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

}
