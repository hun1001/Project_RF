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
    private GameObject[] _tutorialPanels;

    private int _tutorialCount = 0;
    private float _textDuration = 0f;

    [Header("Menu UI")]
    [SerializeField]
    private GameObject _menuUI = null;
    [SerializeField]
    private GameObject _shellList = null;
    [SerializeField]
    private GameObject[] _shellButtons = null;
    [SerializeField]
    private Image[] _shellImages = null;
    [SerializeField]
    private Sprite[] _shellSprites = null;
    [SerializeField]
    private GoodsTexts _menuGoodsText = null;
    [SerializeField]
    private GameObject _tankGameObject = null;
    [SerializeField]
    private GameObject _hanger = null;
    [SerializeField]
    private TextController[] _tankInfos = null;

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
        if (PlayerPrefs.GetInt("Tutorial", 0) == 1) return;
        
        _tutorialPanelParent.SetActive(true);
        _tutorialCount = 0;
        TutorialManager.Instance.TutorialStart();
        CanvasManager.ChangeCanvas(CanvasType.Tutorial);

        if (PlayerPrefs.GetInt("GameTutorial", 0) == 1)
        {
            _tutorialCount = 16;
            _menuGoodsText.SetGoodsTexts(50, 0);
            _techTreeGoodsText.SetGoodsTexts(50, 0);
        }
        _tutorialText.SetText(_textsSO.TutorialTexts[_tutorialCount]);

        if (PlayerDataManager.Instance.GetPlayerTankID() != "BT-5")
        {
            PlayerDataManager.Instance.SetPlayerTankID("BT-5");
        }
        if (_tutorialCount == 0)
        {
            ShellSaveManager.ShellEquip("BT-5", 0, "");
            ShellSaveManager.ShellEquip("BT-5", 1, "");
        }

        _tankInfos[0].SetText("I");
        _tankInfos[1].SetText("BT-5");

        _nextButton.SetActive(true);
    }

    public void TutorialEnd()
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
                    _hanger.SetActive(true);
                    break;
                }
            case 4:
                {
                    _tutorialPanels[0].SetActive(false);
                    _hanger.SetActive(false);
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
                    ShellSaveManager.ShellEquip("BT-5", 0, "HE");
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
                    ShellSaveManager.ShellEquip("BT-5", 1, "AP10");
                    break;
                }
            case 15:
                { 
                    _nextButton.SetActive(false);
                    _tutorialPanels[3].SetActive(true);
                    break;
                }
            case 17:
                {
                    _tutorialPanels[4].SetActive(true);
                    _nextButton.SetActive(false);
                    break;
                }
            case 18:
                {
                    _tutorialPanels[4].SetActive(false);
                    _menuUI.SetActive(false);
                    _techTreeUI.SetActive(true);
                    _nextButton.SetActive(true);
                    break;
                }
            case 19:
                {
                    _nextButton.SetActive(false);
                    _tutorialPanels[5].SetActive(true);
                    break;
                }
            case 20:
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
                    break;
                }
        }
    }
}
