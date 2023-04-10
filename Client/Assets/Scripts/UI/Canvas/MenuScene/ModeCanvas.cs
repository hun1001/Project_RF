using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeCanvas : BaseCanvas
{
    public void OnHomeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Menu);
    }
}
