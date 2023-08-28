using UnityEngine;
using UnityEngine.UI;

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
    private GameObject _nextButton = null;
    [SerializeField]
    private GameObject[] _tutorialPanels;

    private int _tutorialCount = 0;

    [Header("Menu UI")]
    [SerializeField]
    private GameObject _menuUI = null;
    [SerializeField]
    private GameObject _shellList = null;
    [SerializeField]
    private GameObject _filter = null;
    [SerializeField]
    private GameObject[] _shellButtons = null;
    [SerializeField]
    private Image[] _shellImages = null;
    [SerializeField]
    private Sprite[] _shellSprites = null;

    [Header("TechTree UI")]
    [SerializeField]
    private GameObject _techTreeUI = null;

    private void Awake()
    {
        _menuUI.SetActive(true);
        _shellList.SetActive(false);
        _filter.SetActive(false);
        _techTreeUI.SetActive(false);
        _tutorialPanelParent.SetActive(false);
        _shellButtons[0].SetActive(false);
        _shellButtons[1].SetActive(false);
        _nextButton.SetActive(true);
        _skipPanel.SetActive(false);
        foreach (var obj in _tutorialPanels)
        {
            obj.SetActive(false);
        }

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Escape, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType && TutorialManager.Instance.IsTutorial && _skipPanel.activeSelf == false)
            {
                OpenSkipPanel();
            }
        });

        TutorialStart();
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

        if (_tutorialCount == 0)
        {
            TutorialStart();
        }
    }

    public void TutorialStart()
    {
        _tutorialPanelParent.SetActive(true);
        _tutorialCount = 0;
        _tutorialText.SetText(_textsSO.TutorialTexts[0]);
        _nextButton.SetActive(true);
    }

    public void TutorialEnd()
    {
        _tutorialCount = 0;
        TutorialManager.Instance.TutorialEnd();
        CanvasManager.ChangeCanvas(CanvasType.Menu);
    }

    public void GameTutorialStart()
    {
        Debug.Log("Test");
    }

    public void NextTutorial()
    {
        _tutorialCount++;
        if (_tutorialCount == _textsSO.TutorialTexts.Length)
        {
            TutorialEnd();
            return;
        }
        _tutorialText.SetText(_textsSO.TutorialTexts[_tutorialCount]);

        switch (_tutorialCount)
        {
            // Hanger
            case 2:
                {
                    _tutorialPanels[0].SetActive(true);
                    break;
                }
            case 4:
                {
                    _tutorialPanels[0].SetActive(false);
                    break;
                }

            // Shell
            case 5:
                {
                    _tutorialPanels[1].SetActive(true);
                    _nextButton.SetActive(false);
                    break;
                }
            case 6:
                {
                    _shellList.SetActive(true);
                    _tutorialPanels[1].SetActive(false);
                    _tutorialPanels[2].SetActive(true);
                    _nextButton.SetActive(true);
                    break;
                }
            case 7:
                {
                    _nextButton.SetActive(false);
                    _shellButtons[0].SetActive(true);
                    break;
                }
            case 8:
                {
                    _shellList.SetActive(false);
                    _shellButtons[0].SetActive(false);
                    _tutorialPanels[2].SetActive(false);
                    _shellImages[0].sprite = _shellSprites[0];
                    _nextButton.SetActive(true);
                    break;
                }
            case 11:
                {
                    _shellList.SetActive(true);
                    _nextButton.SetActive(false);
                    _tutorialPanels[2].SetActive(true);
                    _shellButtons[1].SetActive(true);
                    break;
                }
            case 12:
                {
                    _nextButton.SetActive(true);
                    _shellList.SetActive(false);
                    _tutorialPanels[2].SetActive(false);
                    _shellButtons[1].SetActive(false);
                    _shellImages[1].sprite = _shellSprites[1];
                    break;
                }
            case 15:
                { 
                    _nextButton.SetActive(false);
                    _tutorialPanels[3].SetActive(true);
                    break;
                }
        }
    }
}
