using Event;
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
    private GameObject _textButton = null;
    [SerializeField]
    private RectTransform _textArea = null;
    [SerializeField]
    private GameObject[] _tutorialPanels;

    private int _tutorialCount = 0;
    private float _textDuration = 0f;
    private bool _isCanReturn = true;

    [Header("Menu UI")]
    [SerializeField]
    private GameObject _menuUI = null;
    [SerializeField]
    private GoodsTexts _menuGoodsText = null;
    
    [Header("Hanger")]
    [SerializeField]
    private GameObject _hanger = null;
    [SerializeField]
    private GameObject _tankGameObject = null;
    [SerializeField]
    private TextController[] _tankInfos = null;

    [Header("Shell")]
    [SerializeField]
    private GameObject _shellList = null;
    [SerializeField]
    private GameObject[] _shellButtons = null;
    [SerializeField]
    private Image[] _shellImages = null;
    [SerializeField]
    private Sprite[] _shellSprites = null;

    [Header("SAT")]
    [SerializeField]
    private GameObject _satList = null;
    [SerializeField]
    private GameObject _satButton = null;
    [SerializeField]
    private Image _satImage = null;
    [SerializeField]
    private Sprite _satSprite = null;
    
    [Header("TechTree UI")]
    [SerializeField]
    private GameObject _techTreeUI = null;
    [SerializeField]
    private GoodsTexts _techTreeGoodsText = null;

    private void Awake()
    {
        foreach (var obj in _tutorialPanels)
        {
            obj.SetActive(false);
        }

        AddInputAction();
    }

    private void Start()
    {
        TutorialStart();
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

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Return, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType && TutorialManager.Instance.IsTutorial)
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

    private void TutorialStart()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) == 1) return;

        _isCanReturn = true;
        _tutorialPanelParent.SetActive(true);
        _tutorialCount = 0;
        TutorialManager.Instance.TutorialStart();
        CanvasManager.ChangeCanvas(CanvasType.Tutorial);

        if (PlayerPrefs.GetInt("GameTutorial", 0) == 1)
        {
            _tutorialCount = 25;
            _menuGoodsText.SetGoodsTexts(50, 0);
            _techTreeGoodsText.SetGoodsTexts(50, 0);
        }
        _tutorialText.SetText(_textsSO.TutorialTexts[_tutorialCount]);

        if (PlayerDataManager.Instance.GetPlayerTankID() != "T-34")
        {
            if (!TechTreeDataManager.HasTank(CountryType.USSR, "T-34"))
            {
                TechTreeDataManager.AddTank(CountryType.USSR, "T-34");
            }
            PlayerDataManager.Instance.SetPlayerTankID("T-34");
        }
        if (_tutorialCount == 0)
        {
            ShellSaveManager.ShellEquip("T-34", 0, "");
            ShellSaveManager.ShellEquip("T-34", 1, "");
            ShellSaveManager.ShellEquip("T-34", 2, "");

            SATSaveManager.SetSAT(string.Empty);
            EventManager.TriggerEvent(EventKeyword.SATReplacement);
        }

        _tankInfos[0].SetText("IV");
        _tankInfos[1].SetText("T-34");

        _nextButton.SetActive(true);
    }

    private void TutorialEnd()
    {
        _tutorialCount = 0;
        PlayerPrefs.SetInt("GameTutorial", 2);
        TutorialManager.Instance.TutorialSkip();
        CanvasManager.ChangeCanvas(CanvasType.Menu);
    }

    public void GameTutorialStart()
    {
        SceneController.ChangeScene("GameTutorialScene");
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
            TutorialEnd();
            return;
        }

        _textDuration = _textsSO.TutorialTexts[_tutorialCount].Length * 0.025f;
        _tutorialText.Typing(_textsSO.TutorialTexts[_tutorialCount], _textDuration);
        _textButton.SetActive(true);

        switch (_tutorialCount)
        {
            // Hanger
            case 2:
                {
                    _tutorialPanels[0].SetActive(true);
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }
            case 3:
                {
                    _tutorialPanels[0].SetActive(false);
                    _tutorialPanels[1].SetActive(true);
                    _nextButton.SetActive(true);
                    _hanger.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
            case 5:
                {
                    _tutorialPanels[2].SetActive(true);
                    break;
                }

            // Shell
            case 6:
                {
                    _tutorialPanels[1].SetActive(false);
                    _tutorialPanels[2].SetActive(false);
                    _hanger.SetActive(false);
                    break;
                }
            case 7:
                {
                    _tutorialPanels[3].SetActive(true);
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }
            case 8:
                {
                    _tutorialPanels[3].SetActive(false);
                    _tutorialPanels[4].SetActive(true);
                    _nextButton.SetActive(true);
                    _shellList.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
            case 9:
                {
                    _shellButtons[0].SetActive(true);
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }
            case 10:
                {
                    ShellSaveManager.ShellEquip("T-34", 0, "HE");
                    _shellImages[0].sprite = _shellSprites[0];
                    _tutorialPanels[4].SetActive(false);
                    _shellButtons[0].SetActive(false);
                    _shellList.SetActive(false);
                    _nextButton.SetActive(true);
                    _isCanReturn = true;
                    break;
                }
            case 13:
                {
                    _tutorialPanels[4].SetActive(true);
                    _shellButtons[1].SetActive(true);
                    _nextButton.SetActive(false);
                    _shellList.SetActive(true);
                    _isCanReturn = false;
                    break;
                }
            case 14:
                {
                    ShellSaveManager.ShellEquip("T-34", 1, "AP10");
                    _shellImages[1].sprite = _shellSprites[1];
                    _tutorialPanels[4].SetActive(false);
                    _shellButtons[1].SetActive(false);
                    _nextButton.SetActive(true);
                    _shellList.SetActive(false);
                    _isCanReturn = true;
                    break;
                }

            // SAT
            case 19:
                {
                    _tutorialPanels[5].SetActive(true);
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }
            case 20:
                {
                    _tutorialPanels[5].SetActive(false);
                    _tutorialPanels[6].SetActive(true);
                    _satButton.SetActive(true);
                    _satList.SetActive(true);
                    break;
                }
            case 21:
                {
                    _tutorialPanels[6].SetActive(false);
                    _satImage.sprite = _satSprite;
                    _satButton.SetActive(false);
                    _nextButton.SetActive(true);
                    _satList.SetActive(false);
                    _isCanReturn = true;

                    SATSaveManager.SetSAT("7.62 mm DT");
                    EventManager.TriggerEvent(EventKeyword.SATReplacement);
                    break;
                }

            // Start
            case 24:
                {
                    _tutorialPanels[7].SetActive(true);
                    _nextButton.SetActive(false);
                    _isCanReturn = false;
                    break;
                }

            // TechTree
            case 200:
                {
                    _tutorialPanels[5].SetActive(false);
                    _techTreeUI.SetActive(false);
                    _nextButton.SetActive(true);
                    _menuUI.SetActive(true);
                    _tankGameObject.SetActive(true);
                    _tankInfos[0].SetText("II");
                    _tankInfos[1].SetText("BT-7");
                    TechTreeDataManager.AddTank(CountryType.USSR, "BT-7");
                    FindObjectOfType<TankModelManager>().ChangeTankModel(Addressable.AddressablesManager.Instance.GetResource<GameObject>("BT-7").GetComponent<Tank>());
                    _menuGoodsText.SetGoodsTexts(0, 0);
                    _techTreeGoodsText.SetGoodsTexts(0, 0);
                    _isCanReturn = true;
                    break;
                }
        }
    }
}
