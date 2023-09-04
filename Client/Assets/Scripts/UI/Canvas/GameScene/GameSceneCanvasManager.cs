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

    public override void ChangeCanvas(CanvasType canvasType, CanvasType beforeCanvas = CanvasType.Base)
    {
        base.ChangeCanvas(canvasType, beforeCanvas);
        if (canvasType == CanvasType.GameTutorial)
        {
            _canvasDictionary[CanvasType.Information].Canvas.enabled = true;
        }
    }
}
