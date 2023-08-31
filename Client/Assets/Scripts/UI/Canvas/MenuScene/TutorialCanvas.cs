using UnityEngine;
using UnityEngine.Rendering.Universal;
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
    private GameObject[] _shellButtons = null;
    [SerializeField]
    private Image[] _shellImages = null;
    [SerializeField]
    private Sprite[] _shellSprites = null;
    [SerializeField]
    private GoodsTexts _menuGoodsText = null;
    [SerializeField]
    private GameObject _tankGameObject = null;

    [Header("TechTree UI")]
    [SerializeField]
    private GameObject _techTreeUI = null;
    [SerializeField]
    private GoodsTexts _techTreeGoodsText = null;

    private void Awake()
    {
        _menuUI.SetActive(true);
        _shellList.SetActive(false);
        _techTreeUI.SetActive(false);
        _tutorialPanelParent.SetActive(false);
        _shellButtons[0].SetActive(false);
        _shellButtons[1].SetActive(false);
        _nextButton.SetActive(true);
        _skipPanel.SetActive(false);
        //_tankGameObject.SetActive(false);
        foreach (var obj in _tutorialPanels)
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
        if (PlayerPrefs.GetInt("Tutorial", 0) == 1) return;

        _tutorialPanelParent.SetActive(true);
        _tutorialCount = 0;
        if (PlayerPrefs.GetInt("GameTutorial", 0) == 1)
        {
            TutorialManager.Instance.TechTreeTutorialStart();
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameTutorialScene");
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
                    //_tankGameObject.SetActive(true);
                    TechTreeDataManager.AddTank(CountryType.USSR, "BT-7");
                    FindObjectOfType<TankModelManager>().ChangeTankModel(Addressable.AddressablesManager.Instance.GetResource<GameObject>("BT-7").GetComponent<Tank>());
                    _menuGoodsText.SetGoodsTexts(0, 0);
                    _techTreeGoodsText.SetGoodsTexts(0, 0);
                    break;
                }
        }
    }
}
