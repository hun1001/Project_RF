using UnityEngine;

public class GameTutorialCanvas : BaseCanvas
{
    [Header("Tutorial")]
    [SerializeField]
    protected TutorialSO _textsSO = null;
    [SerializeField]
    protected GameObject _skipPanel = null;
    [SerializeField]
    protected TextController _tutorialText = null;
    [SerializeField]
    protected GameObject _tutorialPanelParent = null;

    protected int _tutorialCount = 0;

    private void Awake()
    {
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
        _tutorialText.SetText(_textsSO.TutorialTexts[0]);
    }

    public void NextTutorial()
    {
        _tutorialCount++;
        _tutorialText.SetText(_textsSO.TutorialTexts[_tutorialCount]);
    }
}