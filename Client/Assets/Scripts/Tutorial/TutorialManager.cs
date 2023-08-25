using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    // Tutorial ing?
    private bool _isTutorial = false;
    public bool IsTutorial => _isTutorial;

    private void Awake()
    {
        //PlayerPrefs.SetInt("Tutorial", 0);
        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            TutorialStart();
        }
    }

    public void TutorialStart()
    {
        _isTutorial = true;
        PlayerPrefs.SetInt("Tutorial", 1);

        FindObjectOfType<BaseSceneCanvasManager>().ChangeCanvas(CanvasType.Tutorial);
    }

    public void TutorialSkip()
    {
        _isTutorial = false;
    }
}
