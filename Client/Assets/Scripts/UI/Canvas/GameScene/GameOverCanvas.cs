using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : BaseCanvas
{
    public override void OnOpenEvents()
    {
        Time.timeScale = 0f;
        base.OnOpenEvents();
    }
}
