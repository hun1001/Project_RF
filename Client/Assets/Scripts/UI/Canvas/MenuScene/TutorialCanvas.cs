using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialCanvas : BaseCanvas
{
    [Header("Tutorial")]
    [SerializeField]
    private TutorialSO _textsSO = null;
    [SerializeField]
    private GameObject _skipPanel = null;
    [SerializeField]
    private TextController _tutorialText = null;
    [SerializeField]
    private GameObject _tutorialPanelParent = null;
    [SerializeField]
    private GameObject[] _tutorialPanels;

    private int _tutorialCount = 0;

    private void Awake()
    {
        _tutorialPanelParent.SetActive(false);
        _skipPanel.SetActive(true);

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Escape, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType && TutorialManager.Instance.IsTutorial && _skipPanel.activeSelf == false)
            {
                OpenSkipPanel();
            }
        });
    }

    public void OpenSkipPanel()
    {
        PlayButtonSound();
        _skipPanel.SetActive(true);
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
        _tutorialPanelParent.SetActive(true);
    }

    public void TutorialStart()
    {
        _tutorialPanels[0].SetActive(true);
        _tutorialText.SetText(_textsSO.TutorialTexts[0]);
    }

    public void NextTutorial()
    {
        _tutorialPanels[_tutorialCount++].SetActive(false);
        _tutorialText.SetText(_textsSO.TutorialTexts[_tutorialCount]);
        _tutorialPanels[_tutorialCount].SetActive(true);
    }
}
