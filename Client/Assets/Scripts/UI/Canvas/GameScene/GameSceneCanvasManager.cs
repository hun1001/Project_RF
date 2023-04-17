using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCanvasManager : BaseSceneCanvasManager
{
    protected override void Awake()
    {
        base.Awake();
        _activeCanvas = CanvasType.Information;
    }

    public override void ChangeCanvas(CanvasType canvasType, CanvasType beforeCanvas)
    {
        base.ChangeCanvas(canvasType, beforeCanvas);
        if (canvasType != CanvasType.GameItem || canvasType != CanvasType.Setting)
        {
            Time.timeScale = 1f;
            GetCanvas(CanvasType.Controller).Canvas.enabled = GetCanvas(CanvasType.Information).Canvas.enabled = true;
        }
    }
}
