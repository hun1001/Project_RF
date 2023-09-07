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

    public void OnSettingButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Setting, CanvasType);
    }
}
