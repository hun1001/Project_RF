using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialCanvas : TutorialCanvas
{
    protected override void Awake()
    {
        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Escape, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType && TutorialManager.Instance.IsTutorial && _skipPanel.activeSelf == false)
            {
                OpenSkipPanel();
            }
        });
    }

    public override void NextTutorial()
    {
        _tutorialCount++;
        _tutorialText.SetText(_textsSO.TutorialTexts[_tutorialCount]);
    }


}
