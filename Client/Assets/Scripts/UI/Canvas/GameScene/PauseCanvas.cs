using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCanvas : BaseCanvas
{
    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        Time.timeScale = 0f;
    }

    public override void OnCloseEvents()
    {
        base.OnCloseEvents();
        Time.timeScale = 1f;
    }

    public void OnSettingButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Setting, CanvasType);
    }

    public override void OnBackButton()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene)
        {
            base.OnBackButton();
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.GameScene || UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.TrainingScene)
        {
            CanvasManager.ChangeCanvas(CanvasType.Information, CanvasType);
        }
    }
}
