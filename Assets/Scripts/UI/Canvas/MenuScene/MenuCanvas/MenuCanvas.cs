using Addressable;
using DG.Tweening;
using Event;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : BaseCanvas
{
    [SerializeField]
    private GameObject _gameOverPanel = null;

    [Header("Goods")]
    [SerializeField]
    private GoodsTexts _goodsTexts = null;

    [Header("CurrentTankInfo")]
    [SerializeField]
    private MenuTankInfoUI _menuTankInfoUI = null;

    [Header("Shell")]
    [SerializeField]
    private Image[] _shellImages = null;
    private Sprite _plusSprite = null;
    [SerializeField]
    private GameObject _shellReplacement = null;
    [SerializeField]
    private GameObject _shellInformation = null;

    private string _currentTankID
    {
        get
        {
            return PlayerDataManager.Instance.GetPlayerTankID();
        }
    }
    private ShellEquipmentData _shellEquipmentDataDict;

    [Header("SAT")]
    [SerializeField]
    private SATReplacement _satReplacement = null;

    [SerializeField]
    private Image _satImage = null;

    [Header("Animation")]
    [SerializeField]
    private RectTransform _topFrame = null;
    [SerializeField]
    private RectTransform _bottomFrame = null;

    private bool _isHide = false;

    [Header("Hanger")]
    [SerializeField]
    private GameObject _hangerObject = null;
    [SerializeField]
    private Transform _hangerContent = null;
    [SerializeField]
    private GameObject _tankTemplate = null;
    [SerializeField]
    private Sprite[] _flagSprites = null;

    [Header("Filter")]
    [SerializeField]
    private Toggle[] _countryFilterToggles = null;
    private List<bool> _isCountryFilter = null;

    [SerializeField]
    private Toggle[] _tankTypeFilterToggles = null;
    private List<bool> _isTankTypeFilter = null;

    [SerializeField]
    private Toggle[] _tankTierFilterToggles = null;
    private List<bool> _isTankTierFilter = null;

    private Dictionary<string, GameObject> _hangerDict = new Dictionary<string, GameObject>();
    private Dictionary<CountryType, List<GameObject>> _countryHangerDataDict = new Dictionary<CountryType, List<GameObject>>();
    private List<GameObject> _hangerList = new List<GameObject>();

    [Header("Buttons")]
    [SerializeField]
    private Button _startButton = null;

    [Header("Other")]
    [SerializeField]
    private RectTransform _warningPanel;
    private Sequence _warningSequence;
    [SerializeField]
    private GameObject _keyGuidePanel;

    private void Awake()
    {
        GoodsManager.IncreaseFreeGoods(0);
        _goodsTexts.SetGoodsTexts(GoodsManager.FreeGoods, GoodsManager.PaidGoods);

        _startButton.interactable = true;
        _startButton.onClick.AddListener(OnStartButton);

        _plusSprite = AddressablesManager.Instance.GetResource<Sprite>("PlusImage");

        _warningPanel.gameObject.SetActive(false);

        AddInputAction();

        EventManager.StartListening(EventKeyword.MenuCameraMove, CameraUIHide);
        EventManager.StartListening(EventKeyword.ShellReplacement, ShellCheck);
        EventManager.StartListening(EventKeyword.SATReplacement, SATCheck);

        HangerInit();
        FilterInit();
    }

    private void Start()
    {
        _isOpen = true;
        _isHide = false;

        ShellCheck();
        SATCheck();
        HangerUpdate();
        CurrentTankInfoUpdate();
        HangerSort();
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        _isHide = false;
        
        _shellReplacement.SetActive(false);
        _shellInformation.SetActive(false);

        ShellCheck();
        HangerUpdate();
        CurrentTankInfoUpdate();
        HangerSort();
    }

    protected override void AddInputAction()
    {
        KeyboardManager.Instance.AddKeyDownAction(KeyCode.Escape, () =>
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene && TutorialManager.Instance.IsTutorial == false)
            {
                if (CanvasManager.ActiveCanvas == CanvasType)
                {
                    OnOpenGameOver();
                }
            }
        });

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.F1, () =>
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene && TutorialManager.Instance.IsTutorial == false)
            {
                if (CanvasManager.ActiveCanvas == CanvasType)
                {
                    OnOpenGuide();
                }
            }
        });
        KeyboardManager.Instance.AddKeyUpAction(KeyCode.F1, () =>
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == (int)SceneType.MenuScene && TutorialManager.Instance.IsTutorial == false)
            {
                if (CanvasManager.ActiveCanvas == CanvasType)
                {
                    OnOpenGuide();
                }
            }
        });
    }

    public void CurrentTankInfoUpdate()
    {
        _menuTankInfoUI.CurrentTankInfoUpdate();
        _hangerDict[_currentTankID].transform.GetChild(6).gameObject.SetActive(true);
    }

    private void SATCheck()
    {
        string satID = SATSaveManager.SATID;

        if (satID == string.Empty)
        {
            _satImage.sprite = _plusSprite;
        }
        else
        {
            _satImage.sprite = AddressablesManager.Instance.GetResource<GameObject>(SATSaveManager.SATID).GetComponent<BaseSubArmament>().Icon;
        }
    }

    #region Shell
    private void ShellCheck()
    {
        int idx = 0;
        _shellEquipmentDataDict = ShellSaveManager.GetShellEquipment(_currentTankID);

        foreach (var shell in _shellEquipmentDataDict._shellEquipmentList)
        {
            if (shell == "")
            {
                _shellImages[idx++].sprite = _plusSprite;
                continue;
            }

            Shell shellData = AddressablesManager.Instance.GetResource<GameObject>(shell).GetComponent<Shell>();
            _shellImages[idx].sprite = shellData.ShellSprite;
            _shellImages[idx++].gameObject.SetActive(true);
        }
    }

    private bool ShellEmptyCheck()
    {
        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());

        return shellEquipmentData._shellEquipmentList[0] == "" && shellEquipmentData._shellEquipmentList[1] == "" && shellEquipmentData._shellEquipmentList[2] == "";
    }

    private void WarningShellEmpty()
    {
        _warningSequence.Kill();
        _warningSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
            _warningPanel.gameObject.SetActive(true);
            _warningPanel.GetChild(0).GetComponent<TextController>().SetText("No bullets loaded!\nEquip the bullet.");
        })
        .AppendInterval(1.2f)
        .Append(_warningPanel.GetComponent<CanvasGroup>().DOFade(0, 1f))
        .AppendCallback(() =>
        {
            _warningPanel.gameObject.SetActive(false);
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
        });
    }
    #endregion

    #region Hanger
    public void OpenHanger()
    {
        PlayButtonSound();

        _hangerObject.SetActive(!_hangerObject.activeSelf);
    }

    private void HangerInit()
    {
        _countryHangerDataDict.Add(CountryType.USSR, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.Germany, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.USA, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.Britain, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.France, new List<GameObject>());
    }

    public void HangerUpdate()
    {
        TechTreeProgress ussrData = TechTreeDataManager.GetTechTreeProgress(CountryType.USSR);
        TechTreeProgress germanyData = TechTreeDataManager.GetTechTreeProgress(CountryType.Germany);
        TechTreeProgress usaData = TechTreeDataManager.GetTechTreeProgress(CountryType.USA);
        TechTreeProgress britainData = TechTreeDataManager.GetTechTreeProgress(CountryType.Britain);
        TechTreeProgress franceData = TechTreeDataManager.GetTechTreeProgress(CountryType.France);
        TechTreeCanvas techTree = FindObjectOfType<TechTreeCanvas>();

        foreach (var id in ussrData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.transform.GetChild(1).GetComponent<Image>().sprite = GetFlagSprite(CountryType.USSR);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(3).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(4).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(6).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(6).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.USSR].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(6).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }

        foreach (var id in germanyData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.transform.GetChild(1).GetComponent<Image>().sprite = GetFlagSprite(CountryType.Germany);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(3).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(4).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(6).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(6).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.Germany].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(6).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }

        foreach (var id in usaData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.transform.GetChild(1).GetComponent<Image>().sprite = GetFlagSprite(CountryType.USA);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(3).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(4).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(6).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(6).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.USA].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(6).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }

        foreach (var id in britainData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.transform.GetChild(1).GetComponent<Image>().sprite = GetFlagSprite(CountryType.Britain);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(3).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(4).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(6).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(6).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.Britain].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(6).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }

        foreach (var id in franceData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.transform.GetChild(1).GetComponent<Image>().sprite = GetFlagSprite(CountryType.France);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(3).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(4).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(6).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(6).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.France].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(6).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }
    }

    public void HangerSort()
    {
        _hangerList.Clear();

        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.USSR]));
        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.Germany]));
        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.USA]));
        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.Britain]));
        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.France]));

        HangerFilter();
        HangerChangeOrder();
    }

    public void HangerFilter()
    {
        int countryIndex = -1;
        int typeIndex = -1;
        int tierIndex = -1;

        if (_isCountryFilter.Contains(true))
        {
            countryIndex = _isCountryFilter.IndexOf(true);
        }
        if (_isTankTypeFilter.Contains(true))
        {
            typeIndex = _isTankTypeFilter.IndexOf(true);
        }
        if (_isTankTierFilter.Contains(true))
        {
            tierIndex = _isTankTierFilter.IndexOf(true);
        }

        // 모두 비활성화
        if (countryIndex < 0 && typeIndex < 0 && tierIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                _hangerList[i].SetActive(true);
            }
        }
        // 타입 필터
        else if (typeIndex >= 0 && countryIndex < 0 && tierIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.TankType == (TankType)typeIndex)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 나라 필터
        else if (countryIndex >= 0 && typeIndex < 0 && tierIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.CountryType == (CountryType)(countryIndex + 1))
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 티어 필터
        else if (tierIndex >= 0 && typeIndex < 0 && countryIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.TankTier == tierIndex + 1)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 나라, 타입 필터
        else if (countryIndex >= 0 && typeIndex >= 0 && tierIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.CountryType == (CountryType)(countryIndex + 1) && tank.TankSO.TankType == (TankType)typeIndex)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 나라, 티어 필터
        else if (countryIndex >= 0 && typeIndex < 0 && tierIndex >= 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.CountryType == (CountryType)(countryIndex + 1) && tank.TankSO.TankTier == tierIndex + 1)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 티어, 타입 필터
        else if (countryIndex < 0 && typeIndex >= 0 && tierIndex >= 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.TankType == (TankType)typeIndex && tank.TankSO.TankTier == tierIndex + 1)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 셋다 필터
        else
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.CountryType == (CountryType)(countryIndex + 1) && tank.TankSO.TankType == (TankType)typeIndex && tank.TankSO.TankTier == tierIndex + 1)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
    }

    public void FilterInit()
    {
        _isCountryFilter = new List<bool>();
        for (int i = 0; i < _countryFilterToggles.Length; i++)
        {
            int index = i;
            _isCountryFilter.Add(false);
            _countryFilterToggles[index].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    PlayButtonSound();
                    _isCountryFilter[index] = true;
                }
                else
                {
                    _isCountryFilter[index] = false;
                }

                HangerFilter();
            });
        }

        _isTankTypeFilter = new List<bool>();
        for (int i = 0; i < _tankTypeFilterToggles.Length; i++)
        {
            int index = i;
            _isTankTypeFilter.Add(false);
            _tankTypeFilterToggles[index].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    PlayButtonSound();
                    _isTankTypeFilter[index] = true;
                }
                else
                {
                    _isTankTypeFilter[index] = false;
                }

                HangerFilter();
            });
        }

        _isTankTierFilter = new List<bool>();
        for (int i = 0; i < _tankTierFilterToggles.Length; i++)
        {
            int index = i;
            _isTankTierFilter.Add(false);
            _tankTierFilterToggles[index].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    PlayButtonSound();
                    _isTankTierFilter[index] = true;
                }
                else
                {
                    _isTankTierFilter[index] = false;
                }

                HangerFilter();
            });
        }
    }

    public List<GameObject> Sort(List<GameObject> list)
    {
        int gap = list.Count / 2;
        while (gap > 0)
        {
            for (int i = gap; i < list.Count; i++)
            {
                GameObject temp = list[i];
                int j = i;
                while (j >= gap && ShouldSwap(list[j - gap].name, temp.name))
                {
                    list[j] = list[j - gap];
                    j -= gap;
                }
                list[j] = temp;
            }
            gap /= 2;
        }

        return list;
    }

    public void HangerChangeOrder()
    {
        for (int i = 0; i < _hangerList.Count; i++)
        {
            GameObject item = _hangerList[i];

            item.transform.SetSiblingIndex(i);
        }
    }

    private bool ShouldSwap(string a, string b)
    {
        Tank tankA = AddressablesManager.Instance.GetResource<GameObject>(a).GetComponent<Tank>();
        Tank tankB = AddressablesManager.Instance.GetResource<GameObject>(b).GetComponent<Tank>();

        if (tankA != null && tankB != null)
        {
            // 같은 타입일 때만 비교하여 정렬
            if (tankA.TankSO.CountryType == tankB.TankSO.CountryType)
            {
                return tankA.TankSO.TankTier > tankB.TankSO.TankTier;
            }
        }

        return false;
    }

    private Sprite GetFlagSprite(CountryType type)
    {
        switch (type)
        {
            case CountryType.USSR:
                return _flagSprites[0];
            case CountryType.Germany:
                return _flagSprites[1];
            case CountryType.USA:
                return _flagSprites[2];
            case CountryType.Britain:
                return _flagSprites[3];
            case CountryType.France:
                return _flagSprites[4];
            default:
                return null;
        }
    }
    #endregion

    #region Buttons
    public void OnOpenGameOver()
    {
        PlayButtonSound();

        _gameOverPanel.SetActive(!_gameOverPanel.activeSelf);
    }

    public void OnGameOver()
    {
        PlayerPrefs.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnStartButton()
    {
        PlayButtonSound();

        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        _startButton.interactable = false;
        Time.timeScale = 1;
        SceneController.ChangeScene("GameScene");
    }

    public void OnTechTreeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.TechTree, CanvasType);

        PlayButtonSound();
    }

    public void OnOpenShell()
    {
        PlayButtonSound();
        
        _shellReplacement.SetActive(!_shellReplacement.activeSelf);
        _shellInformation.SetActive(false);
    }

    public void OnClickSAT()
    {
        PlayButtonSound();
        _satReplacement.SetActive(!_satReplacement.ActiveSelf);
    }

    public void OnOpenGuide()
    {
        PlayButtonSound();

        _keyGuidePanel.SetActive(!_keyGuidePanel.activeSelf);
    }
#endregion

    #region Animations
    public void CameraUIHide(object[] isHide)
    {
        _isHide = (bool)isHide[0];

        DOTween.Sequence()
        .AppendCallback(() =>
        {
            if (_isHide == false)
            {
                _topFrame.DOAnchorPosY(60f, 0.25f);
                _bottomFrame.DOAnchorPosY(-330f, 0.25f);
            }
            else
            {
                _topFrame.DOAnchorPosY(0f, 0.25f);
                _bottomFrame.DOAnchorPosY(0f, 0.25f);
            }
        });

        if (_hangerObject.activeSelf)
        {
            _hangerObject.SetActive(false);
        }
    }
    #endregion
}
