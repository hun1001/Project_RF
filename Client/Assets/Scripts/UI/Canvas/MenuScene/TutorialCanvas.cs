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
    private GameObject _nextButton = null;
    [SerializeField]
    private GameObject _shellsButton = null;
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
        _skipPanel.SetActive(true);
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
        _tutorialCount = 0;
        //_tutorialPanels[0].SetActive(true);
        _tutorialText.SetText(_textsSO.TutorialTexts[0]);
    }

    public void TutorialEnd()
    {
        _tutorialCount = 0;
        TutorialManager.Instance.TutorialEnd();
        CanvasManager.ChangeCanvas(CanvasType.Menu);
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

        if (_shellList.activeSelf)
        {
            _shellList.SetActive(false);
        }
        if (_filter.activeSelf)
        {
            _filter.SetActive(false);
        }
        
        // Hanger
        if (_tutorialCount == 2)
        {
            _tutorialPanels[0].SetActive(true);
        }
        else if (_tutorialCount <= 4 && _tutorialPanels[0].activeSelf)
        {
            _tutorialPanels[0].SetActive(false);
        }
        // TODO
        if (_tutorialCount == 1234)
        {
            _shellList.SetActive(true);
        }
        if (_tutorialCount == 12345)
        {
            _filter.SetActive(true);
        }
    }
}
