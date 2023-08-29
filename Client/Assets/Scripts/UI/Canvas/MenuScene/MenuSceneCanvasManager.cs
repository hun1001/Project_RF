using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneCanvasManager : BaseSceneCanvasManager
{
    protected override void Awake()
    {
        base.Awake();
        _activeCanvas = CanvasType.Menu;
    }

    protected override void InputEscape()
    {
        if (_openDelay <= 0f)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene && TutorialManager.Instance.IsTutorial == false)
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
        }
    }
}
