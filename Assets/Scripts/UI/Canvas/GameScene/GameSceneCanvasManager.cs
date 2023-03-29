using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCanvasManager : BaseSceneCanvasManager
{
    public override void ChangeCanvas(CanvasType canvasType)
    {
        base.ChangeCanvas(canvasType);
        if (canvasType != CanvasType.Item || canvasType != CanvasType.Setting)
        {
            Time.timeScale = 1f;
            GetCanvas(CanvasType.Controller).Canvas.enabled = GetCanvas(CanvasType.Information).Canvas.enabled = true;
        }
    }
}
