using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class BaseCanvas : MonoBehaviour
{
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

    public void DisableCanvas()
    {
        Canvas.enabled = false;
    }
}
