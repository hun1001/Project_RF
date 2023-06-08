using Addressable;
using DG.Tweening;
using Event;
using Item;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvas : BaseCanvas
{
    [Header("Goods")]
    [SerializeField]
    private GoodsTexts _goodsTexts = null;

    [Header("CurrentTank")]
    [SerializeField]
    private Image _tankTypeImage = null;
    [SerializeField]
    private TextController _tankTierText = null;
    [SerializeField]
    private TextController _tankNameText = null;

    [Header("Gear")]
    // 0 ~ 2 Passive / 3 ~ 4 Active / 5 ~ 6 Shell
    [SerializeField]
    private Image[] _gearImages = null;
    [SerializeField]
    private GameObject[] _lockImages = null;
    private Sprite _plusSprite = null;
    [SerializeField]
    private GameObject _shellReplacement = null;
    private bool _isShellOpen = false;

    private string _currentTankID;
    private ItemEquipmentData _passiveItemEquipmentDataDict;
    //private ItemEquipmentData _activeItemEquipmentDataDict;
    private ShellEquipmentData _shellEquipmentDataDict;

    [Header("Animation")]
    [SerializeField]
    private RectTransform _topFrame;
    [SerializeField]
    private RectTransform _bottomFrame;
    [SerializeField]
    private RectTransform _leftFrame;
    [SerializeField]
    private RectTransform _showButton;
    [SerializeField]
    private RectTransform _hangerDownImage;

    private bool _isHide = false;
    private bool _isHangerHide = false;

    [Header("Hanger")]
    [SerializeField]
    private Transform _hangerContent = null;
    [SerializeField]
    private GameObject _tankTemplate = null;
    [SerializeField]
    private Sprite[] _flagSprites = null;

    private Dictionary<string, GameObject> _hangerDict = new Dictionary<string, GameObject>();
    private Dictionary<CountryType, List<GameObject>> _countryHangerDataDict = new Dictionary<CountryType, List<GameObject>>();
    private List<GameObject> _hangerList = new List<GameObject>();

    [Header("Filter")]
    [SerializeField]
    private GameObject _filterPanel = null;
    private bool _isFilterOpen = false;

    [SerializeField]
    private Toggle[] _sortOrderToggles = null;
    private bool _isSortOrder = false;

    [SerializeField]
    private Toggle[] _countryFilterToggles = null;
    private bool[] _isCountryFilter = null;

    [SerializeField]
    private Toggle[] _tankTypeFilterToggles = null;
    private bool[] _isTankTypeFilter = null;

    [Header("Buttons")]
    [SerializeField]
    private Button _startButton = null;
    [SerializeField]
    private Button _trainingButton = null;
    [SerializeField]
    private Button _serverButton = null;

    [Header("Warning")]
    [SerializeField]
    private RectTransform _warningPanel;
    private Sequence _warningSequence;

    private void Awake()
    {
        GoodsManager.IncreaseFreeGoods(1);
        GoodsManager.IncreaseFreeGoods(-1);

        _goodsTexts.SetGoodsTexts(GoodsManager.FreeGoods, GoodsManager.PaidGoods);

        _startButton.interactable = true;
        _trainingButton.interactable = true;

        _startButton.onClick.AddListener(OnStartButton);
        _trainingButton.onClick.AddListener(OnTrainingStart);
        //_serverButton.onClick.AddListener(OnServerButton);

        _plusSprite = AddressablesManager.Instance.GetResource<Sprite>("PlusImage");

        _warningPanel.gameObject.SetActive(false);

        EventManager.StartListening(EventKeyword.MenuCameraMove, CameraUIHide);

        _isCountryFilter = new bool[_countryFilterToggles.Length];
        for (int i = 0; i < _countryFilterToggles.Length; i++)
        {
            int index = i;
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

        _isTankTypeFilter = new bool[_tankTypeFilterToggles.Length];
        for (int i = 0; i < _tankTypeFilterToggles.Length; i++)
        {
            int index = i;
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
    }

    private void Start()
    {
        _isHide = false;
        _isHangerHide = false;
        _isOpen = true;

        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();

        _countryHangerDataDict.Add(CountryType.USSR, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.Germany, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.USA, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.Britain, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.France, new List<GameObject>());

        EventManager.StartListening(EventKeyword.ShellReplacement, ShellCheck);

        GearCheck();
        HangerUpdate();
        CurrentTankInfoUpdate();
        HangerSort();
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        _isHide = false;
        _isHangerHide = false;
        _isFilterOpen = false;
        _isShellOpen = false;

        _filterPanel.SetActive(false);
        _shellReplacement.SetActive(false);

        _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();

        GearCheck();
        HangerUpdate();
        CurrentTankInfoUpdate();
        HangerSort();
    }

    public override void OnCloseEvents()
    {
        base.OnCloseEvents();

        if (_isHangerHide)
        {
            OnHangerHide();
        }
    }

    public void CurrentTankInfoUpdate()
    {
        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();
        Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_currentTankID).GetComponent<Tank>();
        TechTree techTree = FindObjectOfType<TechTree>();

        _tankTypeImage.sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
        _tankTierText.SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);
        _tankNameText.SetText(_currentTankID);
        
        _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(true);
    }

    public void GearCheck()
    {
        Tank currentTank = FindObjectOfType<TankModelManager>().TankModel;
        _passiveItemEquipmentDataDict = ItemSaveManager.GetItemEquipment(ItemType.Passive);
        //_activeItemEquipmentDataDict = ItemSaveManager.GetItemEquipment(ItemType.Active);

        uint passiveSlotSize = currentTank.TankSO.PassiveItemInventorySize;
        uint idx = 0;
        foreach (var passive in _passiveItemEquipmentDataDict._itemEquipmentList)
        {
            if (idx + 1 > passiveSlotSize)
            {
                _gearImages[idx].sprite = null;
                _gearImages[idx].gameObject.SetActive(false);
                _lockImages[idx++].SetActive(true);
                continue;
            }

            _lockImages[idx].SetActive(false);
            if (passive == "")
            {
                _gearImages[idx++].sprite = _plusSprite;
                continue;
            }

            Item_Base itemInfo = AddressablesManager.Instance.GetResource<GameObject>(passive).GetComponent<Item_Base>();
            _gearImages[idx].sprite = itemInfo.ItemSO.Image;
            _gearImages[idx++].gameObject.SetActive(true);
        }

        /*uint activeSlotSize = currentTank.TankSO.ActiveItemInventorySize;
        foreach (var active in _activeItemEquipmentDataDict._itemEquipmentList)
        {
            if (idx - 2 > activeSlotSize)
            {
                _gearImages[idx].sprite = null;
                _gearImages[idx].gameObject.SetActive(false);
                _lockImages[idx++].SetActive(true);
                continue;
            }

            _lockImages[idx].SetActive(false);
            if (active == "")
            {
                _gearImages[idx].sprite = null;
                _gearImages[idx++].gameObject.SetActive(false);
                continue;
            }

            Item_Base itemInfo = AddressablesManager.Instance.GetResource<GameObject>(active).GetComponent<Item_Base>();
            _gearImages[idx].sprite = itemInfo.ItemSO.Image;
            _gearImages[idx++].gameObject.SetActive(true);
        }*/

        ShellCheck();
    }

    private void ShellCheck()
    {
        int idx = 3;
        _shellEquipmentDataDict = ShellSaveManager.GetShellEquipment(_currentTankID);

        foreach (var shell in _shellEquipmentDataDict._shellEquipmentList)
        {
            if (shell == "")
            {
                _gearImages[idx++].sprite = _plusSprite;
                continue;
            }

            Shell shellData = AddressablesManager.Instance.GetResource<GameObject>(shell).GetComponent<Shell>();
            _gearImages[idx].sprite = shellData.ShellSprite;
            _gearImages[idx++].gameObject.SetActive(true);
        }
    }

    private void HangerUpdate()
    {
        TechTreeProgress ussrData = TechTreeDataManager.GetTechTreeProgress(CountryType.USSR);
        TechTreeProgress germanyData = TechTreeDataManager.GetTechTreeProgress(CountryType.Germany);
        TechTreeProgress usaData = TechTreeDataManager.GetTechTreeProgress(CountryType.USA);
        TechTreeProgress britainData = TechTreeDataManager.GetTechTreeProgress(CountryType.Britain);
        TechTreeProgress franceData = TechTreeDataManager.GetTechTreeProgress(CountryType.France);
        TechTree techTree = FindObjectOfType<TechTree>();

        foreach (var id in ussrData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.USSR);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.USSR].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                GearCheck();
            });
        }

        foreach (var id in germanyData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.Germany);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.Germany].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                GearCheck();
            });
        }

        foreach (var id in usaData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.USA);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.USA].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                GearCheck();
            });
        }

        foreach (var id in britainData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.Britain);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.Britain].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                GearCheck();
            });
        }

        foreach (var id in franceData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.France);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.France].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                GearCheck();
            });
        }
    }

    private void HangerSort()
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

    private void HangerFilter()
    {
        int countryIndex = -1;
        int typeIndex = -1;
        for (int i = 0; i < _isCountryFilter.Length; i++)
        {
            if (_isCountryFilter[i])
            {
                countryIndex = i;
                break;
            }
        }
        for (int i = 0; i < _isTankTypeFilter.Length; i++)
        {
            if (_isTankTypeFilter[i])
            {
                typeIndex = i;
                break;
            }
        }

        // 둘다 비활성화
        if (countryIndex < 0 && typeIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                _hangerList[i].SetActive(true);
            }
        }
        // 타입 필터
        else if (typeIndex >= 0 && countryIndex < 0)
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
        else if (countryIndex >= 0 && typeIndex < 0)
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
        // 둘다 필터
        else
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
    }

    private List<GameObject> Sort(List<GameObject> list)
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

    public void OnSortOrder(bool isOrder)
    {
        _isSortOrder = isOrder;
        PlayButtonSound();

        if (isOrder)
        {
            _sortOrderToggles[1].isOn = true;
        }
        else
        {
            _sortOrderToggles[0].isOn = true;
        }

        HangerSort();
    }

    public void OnFilterOpen()
    {
        PlayButtonSound();
        if (_isFilterOpen)
        {
            _isFilterOpen = false;
            _filterPanel.SetActive(false);
        }
        else
        {
            _isFilterOpen = true;
            _filterPanel.SetActive(true);
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
                if (_isSortOrder)
                {
                    return tankA.TankSO.TankTier < tankB.TankSO.TankTier;
                }
                else
                {
                    return tankA.TankSO.TankTier > tankB.TankSO.TankTier;
                }
            }
        }

        return false;
    }

    private void HangerChangeOrder()
    {
        for (int i = 0; i < _hangerList.Count; i++)
        {
            GameObject item = _hangerList[i];

            item.transform.SetSiblingIndex(i);
        }
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

    public void OnStartButton()
    {
        PlayButtonSound();

        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        PlayButtonSound();

        _startButton.interactable = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
        Pool.PoolManager.DeleteAllPool();
        EventManager.ClearEvent();
    }

    public void OnTrainingStart()
    {
        PlayButtonSound();

        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        _trainingButton.interactable = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("TrainingScene");
        Pool.PoolManager.DeleteAllPool();
        EventManager.ClearEvent();
    }

    public void OnServerButton()
    {
        PlayButtonSound();

        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        _serverButton.interactable = false;
        ServerManager.Instance.ConnectToServer();
        EventManager.ClearEvent();
    }

    private bool ShellEmptyCheck()
    {
        ShellEquipmentData shellEquipmentData = ShellSaveManager.GetShellEquipment(PlayerDataManager.Instance.GetPlayerTankID());

        return shellEquipmentData._shellEquipmentList[0] == "" && shellEquipmentData._shellEquipmentList[1] == "";
    }

    private void WarningShellEmpty()
    {
        _warningSequence.Kill();
        _warningSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
            _warningPanel.gameObject.SetActive(true);
            _warningPanel.GetChild(0).GetComponent<TextController>().SetText("총알이 장착되어 있지 않습니다!\n총알을 장착해주세요.");
        })
        .AppendInterval(1.2f)
        .Append(_warningPanel.GetComponent<CanvasGroup>().DOFade(0, 1f))
        .AppendCallback(() =>
        {
            _warningPanel.gameObject.SetActive(false);
            _warningPanel.GetComponent<CanvasGroup>().DOFade(1, 0f);
        });
    }

    public void OnModeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Mode, CanvasType);

        PlayButtonSound();
    }

    public void OnSettingButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Setting, CanvasType);

        PlayButtonSound();
    }

    public void OnShopButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Shop, CanvasType);

        PlayButtonSound();
    }

    public void OnTechTreeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.TechTree, CanvasType);

        PlayButtonSound();
    }

    public void OnOpenItem()
    {
        CanvasManager.ChangeCanvas(CanvasType.Gear, CanvasType);

        PlayButtonSound();
    }

    public void OnOpenShell()
    {
        PlayButtonSound();
        if (_isShellOpen)
        {
            _isShellOpen = false;
            _shellReplacement.SetActive(false);
        }
        else
        {
            _isShellOpen = true;
            _shellReplacement.SetActive(true);
        }
    }

    public void OnHangerHide()
    {
        if (_isHangerHide)
        {
            _isHangerHide = false;
            _bottomFrame.DOAnchorPosY(0f, 0.3f);
            _hangerDownImage.DORotate(Vector3.zero, 0.3f);
        }
        else
        {
            _isHangerHide = true;
            _bottomFrame.DOAnchorPosY(-68f, 0.3f);
            _hangerDownImage.DORotate(Vector3.forward * 180f, 0.3f);
        }

        PlayButtonSound();
    }

    public void UIHide(bool isHide)
    {
        _isHide = isHide;

        _startSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            if (_isHide == false)
            {
                _topFrame.DOAnchorPosY(82f, 0.25f);
                _bottomFrame.DOAnchorPosY(-102f, 0.25f);
                _leftFrame.DOAnchorPosX(-60f, 0.25f);
                //_showButton.DOAnchorPosY(-_showButton.sizeDelta.y, 0.25f);
            }
            else
            {
                _topFrame.DOAnchorPosY(0f, 0.25f);
                if (_isHangerHide)
                {
                    _bottomFrame.DOAnchorPosY(-68f, 0.25f);
                }
                else
                {
                    _bottomFrame.DOAnchorPosY(0f, 0.25f);
                }
                _leftFrame.DOAnchorPosX(0f, 0.25f);
                //_showButton.DOAnchorPosY(0f, 0.25f);
            }
        });
    }

    public void CameraUIHide(object[] isHide)
    {
        _isHide = (bool)isHide[0];

        _startSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            if (_isHide == false)
            {
                _topFrame.DOAnchorPosY(82f, 0.25f);
                _bottomFrame.DOAnchorPosY(-102f, 0.25f);
                _leftFrame.DOAnchorPosX(-60f, 0.25f);
            }
            else
            {
                _topFrame.DOAnchorPosY(0f, 0.25f);
                if (_isHangerHide)
                {
                    _bottomFrame.DOAnchorPosY(-68f, 0.25f);
                }
                else
                {
                    _bottomFrame.DOAnchorPosY(0f, 0.25f);
                }
                _leftFrame.DOAnchorPosX(0f, 0.25f);
            }
        });

        if (_isFilterOpen)
        {
            OnFilterOpen();
        }
    }
}
