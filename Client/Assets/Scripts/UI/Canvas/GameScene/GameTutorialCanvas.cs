using Event;
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
    [SerializeField]
    private GameObject _textButton = null;
    [SerializeField]
    private GameObject[] _tutorialPanels = null;

    private int _tutorialCount = 0;
    private float _textDuration = 0f;

    private void Awake()
    {
        EventManager.StartListening(EventKeyword.NextTutorial, NextTutorial);

        foreach (var obj in  _tutorialPanels)
        {
            obj.SetActive(false);
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
    }

    private void Start()
    {
        TutorialManager.Instance.GameTutorialStart();
        TutorialStart();
    }

    private void Update()
    {
        if (_textDuration > 0f)
        {
            _textDuration -= Time.deltaTime;

            if (_textDuration <= 0f)
            {
                _textButton.SetActive(false);
            }
        }
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

    public void TextCancel()
    {
        _tutorialText.SetText(_textsSO.TutorialTexts[_tutorialCount]);
        _textDuration = 0f;
        _textButton.SetActive(false);
    }

    public void NextTutorial()
    {
        _tutorialCount++;
        if (_tutorialCount == _textsSO.TutorialTexts.Length)
        {
            if (PlayerPrefs.GetInt("GameTutorial", 0) < 2)
            {
                PlayerPrefs.SetInt("GameTutorial", 1);
            }
            CanvasManager.ChangeCanvas(CanvasType.Information);
            TutorialManager.Instance.TutorialWaveStart();
            return;
        }

        _textDuration = _textsSO.TutorialTexts[_tutorialCount].Length * 0.025f;
        _tutorialText.Typing(_textsSO.TutorialTexts[_tutorialCount], _textDuration);
        _textButton.SetActive(true);

        switch (_tutorialCount)
        {
            case 4:
                {
                    _tutorialPanels[0].SetActive(true);
                    break;
                }
            case 5:
                {
                    _tutorialPanels[0].SetActive(false);
                    _tutorialPanels[1].SetActive(true);
                    break;
                }
            case 6:
                {
                    _tutorialPanels[1].SetActive(false);
                    _nextButton.SetActive(false);
                    TutorialManager.Instance.MovingTargetSpawn();
                    break;
                }
            case 7:
                {
                    _nextButton.SetActive(true);
                    break;
                }
            case 8:
                {
                    TutorialManager.Instance.TankDummySpawn();
                    break;
                }
            case 9:
                {
                    _tutorialPanels[1].SetActive(true);
                    break;
                }
            case 10:
                {
                    _tutorialPanels[1].SetActive(false);
                    _nextButton.SetActive(false);
                    break;
                }
            case 11:
                {
                    _nextButton.SetActive(true);
                    break;
                }
        }
    }
}