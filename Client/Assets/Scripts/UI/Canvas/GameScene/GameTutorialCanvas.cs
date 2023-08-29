using UnityEngine;

public class GameTutorialCanvas : BaseCanvas
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

    protected int _tutorialCount = 0;

    private void Awake()
    {
        if (CanvasManager.ActiveCanvas == CanvasType)
        {
            CanvasManager.ChangeCanvas(CanvasType);
        }

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Escape, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType && TutorialManager.Instance.IsTutorial)
            {
                if (_skipPanel.activeSelf == false)
                {
                    OpenSkipPanel();
                }
                else
                {
                    TutorialNotSkip();
                }
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
        _tutorialCount = 0;
        TutorialManager.Instance.TutorialSkip();
        OnHomeButton();
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

    public void NextTutorial()
    {
        _tutorialCount++;
        if (_tutorialCount == _textsSO.TutorialTexts.Length)
        {
            CanvasManager.ChangeCanvas(CanvasType.Information);
            return;
        }
        _tutorialText.SetText(_textsSO.TutorialTexts[_tutorialCount]);

        switch (_tutorialCount)
        {

        }
    }
}