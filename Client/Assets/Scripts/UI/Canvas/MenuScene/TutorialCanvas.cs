using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : BaseCanvas
{
    [Header("Tutorial")]
    [SerializeField]
    private GameObject _skipPanel = null;

    private int _tutorialCount = 0;

    private void Awake()
    {
        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Escape, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType && TutorialManager.Instance.IsTutorial && _skipPanel.activeSelf == false)
            {
                _skipPanel.SetActive(true);
            }
        });
    }

    public void TutorialSkip()
    {
        PlayButtonSound();
        _tutorialCount = 0;
        TutorialManager.Instance.TutorialSkip();
        CanvasManager.ChangeCanvas(CanvasType.Menu);
    }

    public void TutorialNotSkip()
    {
        PlayButtonSound();
        _skipPanel.SetActive(false);
    }
}
