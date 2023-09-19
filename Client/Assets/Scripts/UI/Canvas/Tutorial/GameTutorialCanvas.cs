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
    private GameObject _nextButton = null;
    [SerializeField]
    private GameObject _textButton = null;
    [SerializeField]
    private RectTransform _textArea = null;
    [SerializeField]
    private GameObject[] _tutorialPanels = null;

    private int _tutorialCount = 0;
    private float _textDuration = 0f;
    private bool _isCanReturn = true;
    private bool _isHE = false;
    private bool _isAP = false;

    private void Awake()
    {
        TutorialManager.Instance.GameTutorialStart();
        TutorialStart();

        EventManager.StartListening(EventKeyword.NextTutorial, NextTutorial);

        foreach (var obj in  _tutorialPanels)
        {
            obj.SetActive(false);
        }
    }

    private void Start()
    {
        AddInputAction();
    }

    private void Update()
    {
        if (_textDuration > 0f)
        {
            _textDuration -= Time.unscaledDeltaTime;

            if (_textDuration <= 0f)
            {
                _textButton.SetActive(false);
            }
        }
    }

    protected override void AddInputAction()
    {
        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Escape, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
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
            else if (CanvasManager.ActiveCanvas != CanvasType.GameOver)
            {
                CanvasManager.ChangeCanvas(CanvasType);
                OpenSkipPanel();
            }
        });

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Return, () =>
        {
            if (TutorialManager.Instance.IsTutorial)
            {
                if (_textDuration > 0f)
                {
                    TextCancel();
                }
                else if (_isCanReturn)
                {
                    NextTutorial();
                }
            }
        });

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Alpha1, () =>
        {
            if (TutorialManager.Instance.IsTutorial)
            {
                if (_isHE)
                {
                    _isHE = false;
                    TutorialManager.Instance.IsCanAttack = true;
                    TutorialManager.Instance.IsCanChangeShell = false;
                }
            }
        });

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Alpha2, () =>
        {
            if (TutorialManager.Instance.IsTutorial)
            {
                if (_isAP)
                {
                    _isAP = false;
                    TutorialManager.Instance.IsCanAttack = true;
                    TutorialManager.Instance.IsCanChangeShell = false;
                }
            }
        });
    }

    public void OpenSkipPanel()
    {
        PlayButtonSound();

        Time.timeScale = 0f;
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
        Time.timeScale = 1f;

        if (PlayerPrefs.GetInt("GameTutorial", 0) == 1)
        {
            CanvasManager.ChangeCanvas(CanvasType.Information);
        }
    }

    private void TutorialStart()
    {
        _textArea.gameObject.SetActive(true);
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

    public void ControlTypeChange(int type)
    {
        if (type == 0)
        {
            PlayerPrefs.SetInt("ControlType", 0);
            EventManager.TriggerEvent(EventKeyword.ChangeControlType, 0);
        }
        else
        {
            PlayerPrefs.SetInt("ControlType", 1);
            EventManager.TriggerEvent(EventKeyword.ChangeControlType, 1);
        }
    }

    private void TriggerNextTutorial()
    {
        EventManager.TriggerEvent(EventKeyword.NextTutorial);
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

            _textArea.gameObject.SetActive(false);
            CanvasManager.ChangeCanvas(CanvasType.Information);
            TutorialManager.Instance.TutorialWaveStart();
            return;
        }

        _textDuration = _textsSO.TutorialTexts[_tutorialCount].Length * 0.025f;
        _tutorialText.Typing(_textsSO.TutorialTexts[_tutorialCount], _textDuration);
        _textButton.SetActive(true);

        switch (_tutorialCount)
        {
            case 1:
                {
                    VirtualCameraManager.Instance.SwitchingCamera();
                    break;
                }
            case 2:
                {
                    VirtualCameraManager.Instance.SwitchingCamera(false);
                    TutorialManager.Instance.DummyRemove();
                    break;
                }
            case 6:
                {
                    TutorialManager.Instance.IsCanMove = true;
                    _tutorialPanels[0].SetActive(true);

                    PlayerPrefs.SetInt("ControlType", 0);
                    EventManager.TriggerEvent(EventKeyword.ChangeControlType, 0);
                    break;
                }
            case 7:
                {
                    PlayerPrefs.SetInt("ControlType", 1);
                    EventManager.TriggerEvent(EventKeyword.ChangeControlType, 1);
                    break;
                }
            case 8:
                {
                    TutorialManager.Instance.IsCanMove = false;
                    _tutorialPanels[0].SetActive(false);
                    _tutorialPanels[1].SetActive(true);
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }
            case 9:
                {
                    _tutorialPanels[1].SetActive(false);
                    _tutorialPanels[2].SetActive(true);
                    _nextButton.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
            case 11:
                {
                    _tutorialPanels[2].SetActive(false);
                    break;
                }
            case 12:
                {
                    TutorialManager.Instance.MovingTargetSpawn();
                    TutorialManager.Instance.IsCanMove = true;
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }
            case 13:
                {
                    TutorialManager.Instance.IsCanMove = false;
                    _nextButton.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
            case 14:
                {
                    TutorialManager.Instance.TankDummySpawn("BT-5", new Vector3(-56f, 6f, 0));
                    TutorialManager.Instance.TankDummyMove(new Vector3(-14f, -1f, 0f));
                    break;
                }
            case 17:
                {
                    TutorialManager.Instance.IsCanChangeShell = true;
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    _isHE = true;
                    break;
                }
            case 18:
                {
                    TutorialManager.Instance.IsCanAttack = false;
                    _nextButton.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
            case 19:
                {
                    TutorialManager.Instance.TankDummySpawn("Pz.IV H", new Vector3(56f, -25f, 0f));
                    TutorialManager.Instance.TankDummyMove(new Vector3(44f, -14f, 0));
                    break;
                }
            case 21:
                {
                    EventManager.StartListening("Ricochet", TriggerNextTutorial);
                    TutorialManager.Instance.IsCanChangeShell = true;
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    _isAP = true;
                    break;
                }
            case 22:
                {
                    EventManager.DeleteEvent("Ricochet");
                    TutorialManager.Instance.IsCanAttack = false;
                    _nextButton.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
            case 24:
                {
                    TutorialManager.Instance.TankDummyMove(new Vector3(47f, -24f, 0));
                    TutorialManager.Instance.IsCanAttack = true;
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }
            case 25:
                {
                    TutorialManager.Instance.IsCanAttack = false;
                    _nextButton.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
        }
    }
}