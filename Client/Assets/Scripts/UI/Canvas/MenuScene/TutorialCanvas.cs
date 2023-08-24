using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
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

    [Header("Menu UI")]
    [SerializeField]
    private GameObject _menuUI = null;
    [SerializeField]
    private GameObject _shellList = null;
    [SerializeField]
    private GameObject _filter = null;

    [Header("TechTree UI")]
    [SerializeField]
    private GameObject _techTreeUI = null;

    private int _tutorialCount = 0;

    private void Awake()
    {
        _menuUI.SetActive(true);
        _shellList.SetActive(false);
        _filter.SetActive(false);
        _techTreeUI.SetActive(false);
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
        _tutorialCount = 0;
        _tutorialPanels[0].SetActive(true);
        _tutorialText.SetText(_textsSO.TutorialTexts[0]);
    }

    public void NextTutorial()
    {
        _tutorialPanels[_tutorialCount++].SetActive(false);
        _tutorialText.SetText(_textsSO.TutorialTexts[_tutorialCount]);
        _tutorialPanels[_tutorialCount].SetActive(true);

        if (_tutorialCount == 1234)
        {
            _shellList.SetActive(true);
        }
        if (_tutorialCount == 12345)
        {
            _filter.SetActive(true);
        }
        if (_shellList.activeSelf)
        {
            _shellList.SetActive(false);
        }
        if (_filter.activeSelf)
        {
            _filter.SetActive(false);
        }
    }
}
