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
}
