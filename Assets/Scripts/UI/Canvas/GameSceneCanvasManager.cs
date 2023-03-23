using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCanvasManager : BaseSceneCanvasManager
{
    public override void ChangeCanvas(CanvasType canvasType)
    {
        switch (canvasType)
        {
            case CanvasType.Controller:
                break;
            case CanvasType.Item:
                break;
            case CanvasType.Information:
                break;
            default:
                Debug.LogError("CanvasType is not defined");
                break;
        }
    }
}
