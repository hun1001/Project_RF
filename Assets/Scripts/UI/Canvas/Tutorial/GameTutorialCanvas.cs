using DG.Tweening;
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

    [Header("Move")]
    [SerializeField]
    private TextController _confirmedText = null;
    [SerializeField]
    private RectTransform _selectedType = null;
    [SerializeField]
    private TutorialArrowHandle _arrow = null;
    [SerializeField]
    private RectTransform[] _textColor = null;

    private int _tutorialCount = 0;
    private int _controlType = 0;
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
        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Space, () =>
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

    public void SelectControlType(int type)
    {
        _controlType = type;
     
        PlayerPrefs.SetInt("ControlType", _controlType);
        EventManager.TriggerEvent(EventKeyword.ChangeControlType, _controlType);

        if (_controlType == 0) // Simple
        {
            _tutorialCount = 6;
        }
        else if ( _controlType == 1) // Detail
        {
            _tutorialCount = 8;
        }

        NextTutorial();
    }

    public void NotConfirmed()
    {
        _tutorialPanels[2].SetActive(false);
        _tutorialCount = 5;
        NextTutorial();
    }

    public void OpenExperience(int type)
    {
        PlayButtonSound();

        _controlType = type;
        PlayerPrefs.SetInt("ControlType", _controlType);
        EventManager.TriggerEvent(EventKeyword.ChangeControlType, _controlType);

        _textArea.gameObject.SetActive(false);
        _tutorialPanels[1].SetActive(false);
        _tutorialPanels[2].SetActive(false);
        _tutorialPanels[3].SetActive(true);

        TutorialManager.Instance.IsCanMove = true;

        if (_controlType == 0)
        {
            _selectedType.DOAnchorPosX(10f, 0.7f);
            _textColor[0].DOAnchorPosX(0f, 0.7f);
            _textColor[1].DOAnchorPosX(-220f, 0.7f);
        }
        else if (_controlType == 1)
        {
            _selectedType.DOAnchorPosX(230f, 0.7f);
            _textColor[0].DOAnchorPosX(220f, 0.7f);
            _textColor[1].DOAnchorPosX(0f, 0.7f);

        }
    }

    public void ExitExperience()
    {
        PlayButtonSound();

        _tutorialPanels[1].SetActive(true);
        _tutorialPanels[3].SetActive(false);
        _textArea.gameObject.SetActive(true);

        TutorialManager.Instance.IsCanMove = false;
    }

    public void ChangeControlType()
    {
        if (_controlType == 0)
        {
            _controlType = 1;

            _selectedType.DOAnchorPosX(230f, 0.7f);
            _textColor[0].DOAnchorPosX(220f, 0.7f);
            _textColor[1].DOAnchorPosX(0f, 0.7f);

        }
        else if ( _controlType == 1)
        {
            _controlType = 0;

            _selectedType.DOAnchorPosX(10f, 0.7f);
            _textColor[0].DOAnchorPosX(0f, 0.7f);
            _textColor[1].DOAnchorPosX(-220f, 0.7f);
        }

        PlayerPrefs.SetInt("ControlType", _controlType);
        EventManager.TriggerEvent(EventKeyword.ChangeControlType, _controlType);
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

            _isCanReturn = false;
            _textArea.gameObject.SetActive(false);
            CanvasManager.ChangeCanvas(CanvasType.Information);
            TutorialManager.Instance.TutorialWaveStart();
            return;
        }

        PlayButtonSound();
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
            // Move
            case 6:
                {
                    _tutorialPanels[0].SetActive(true);

                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }
            case 7:
            case 9:
                {
                    _tutorialPanels[0].SetActive(false);

                    _nextButton.SetActive(true);
                    _isCanReturn = true;

                    break;
                }
            case 8:
            case 10:
                {
                    _tutorialCount = 10;
                    _tutorialPanels[1].SetActive(true);
                   
                    break;
                }
            case 11:
                {
                    _tutorialPanels[1].SetActive(false);

                    TutorialMoveTarget target = TutorialManager.Instance.MovingTargetSpawn(0);
                    _arrow.SetTarget(target);

                    TutorialManager.Instance.IsCanMove = true;

                    _nextButton.SetActive(false);
                    _isCanReturn = false;

                    break;
                }
            case 13:
                {
                    TutorialMoveTarget target = TutorialManager.Instance.MovingTargetSpawn(1);
                    _arrow.SetTarget(target);

                    TutorialManager.Instance.IsCanMove = true;

                    _nextButton.SetActive(false);
                    _isCanReturn = false;

                    break;
                }
            case 12:
            case 14:
                {
                    TutorialManager.Instance.IsCanMove = false;
                    _nextButton.SetActive(true);
                    _isCanReturn = true;

                    break;
                }
            case 15:
                {
                    _tutorialPanels[2].SetActive(true);
                    if (PlayerPrefs.GetInt("ControlType") == 0)
                    {
                        _confirmedText.SetText("Simple 타입으로 진행하시겠습니까?");
                    }
                    else if (PlayerPrefs.GetInt("ControlType") == 1)
                    {
                        _confirmedText.SetText("Detail 타입으로 진행하시겠습니까?");
                    }

                    _nextButton.SetActive(false);
                    _isCanReturn = false;

                    break;
                }
            case 16:
                {
                    _tutorialPanels[2].SetActive(false);

                    _nextButton.SetActive(true);
                    _isCanReturn = true;

                    break;
                }

                // Attack
            case 19:
                {
                    TutorialManager.Instance.TankDummySpawn("BT-5", new Vector3(-56f, 6f, 0));
                    TutorialManager.Instance.TankDummyMove(new Vector3(-14f, -1f, 0f));
                    break;
                }
            case 23:
                {
                    TutorialManager.Instance.IsCanChangeShell = true;
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    _isHE = true;
                    break;
                }
            case 24:
                {
                    TutorialManager.Instance.IsCanAttack = false;
                    _nextButton.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
            case 25:
                {
                    TutorialManager.Instance.TankDummySpawn("Pz.IV H", new Vector3(56f, -25f, 0f));
                    TutorialManager.Instance.TankDummyMove(new Vector3(44f, -14f, 0));
                    break;
                }
            case 27:
                {
                    EventManager.StartListening("Ricochet", TriggerNextTutorial);
                    TutorialManager.Instance.IsCanChangeShell = true;
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    _isAP = true;
                    break;
                }
            case 28:
                {
                    EventManager.DeleteEvent("Ricochet");
                    TutorialManager.Instance.IsCanAttack = false;
                    _nextButton.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
            case 30:
                {
                    TutorialManager.Instance.TankDummyMove(new Vector3(47f, -24f, 0));
                    TutorialManager.Instance.IsCanAttack = true;
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }
            case 31:
                {
                    TutorialManager.Instance.IsCanAttack = false;
                    _nextButton.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
        }
    }
}