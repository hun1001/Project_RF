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
        if (canvasType == CanvasType.GameTutorial)
        {
            GetCanvas(CanvasType.Information).Canvas.enabled = true;
        }
    }

    protected override void InputEscape()
    {
        if (_openDelay <= 0f)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene && TutorialManager.Instance.IsTutorial == false)
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
}
