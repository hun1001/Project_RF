using Addressable;
using DG.Tweening;
using Event;
using Item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    private Transform _hangerContent;
    [SerializeField]
    private GameObject _tankTemplate;

    private Dictionary<CountryType, Sprite> _flagImageDict = new Dictionary<CountryType, Sprite>();

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
    [SerializeField]
    private TextController _warningText;
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

        _flagImageDict.Add(CountryType.USSR, AddressablesManager.Instance.GetResource<Sprite>("USSRFlagImage"));
        _flagImageDict.Add(CountryType.Germany, AddressablesManager.Instance.GetResource<Sprite>("GermanyFlagImage"));
        _flagImageDict.Add(CountryType.USA, AddressablesManager.Instance.GetResource<Sprite>("USAFlagImage"));
        _flagImageDict.Add(CountryType.Britain, AddressablesManager.Instance.GetResource<Sprite>("BritainFlagImage"));
        _flagImageDict.Add(CountryType.France, AddressablesManager.Instance.GetResource<Sprite>("FranceFlagImage"));

        _warningPanel.gameObject.SetActive(false);

        EventManager.StartListening(EventKeyword.MenuCameraMove, CameraUIHide);
    }

    private void Start()
    {
        _isHide = false;
        _isHangerHide = false;
        _isOpen = true;

        GearCheck();
        HangerUpdate();
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        _isHide = false;
        _isHangerHide = false;

        CurrentTankInfoUpdate();
        GearCheck();
        HangerUpdate();
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
        Tank tank = AddressablesManager.Instance.GetResource<GameObject>(PlayerDataManager.Instance.GetPlayerTankID()).GetComponent<Tank>();
        TechTree techTree = FindObjectOfType<TechTree>();

        _tankTypeImage.sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
        _tankTierText.SetText(techTree.TankTierNumber[techTree.TankTier]);
        _tankNameText.SetText(tank.ID);
    }

    public void GearCheck()
    {
        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();
        Tank currentTank = FindObjectOfType<TankModelManager>().TankModel;
        _passiveItemEquipmentDataDict = ItemSaveManager.GetItemEquipment(ItemType.Passive);
        //_activeItemEquipmentDataDict = ItemSaveManager.GetItemEquipment(ItemType.Active);
        _shellEquipmentDataDict = ShellSaveManager.GetShellEquipment(_currentTankID);

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

        //uint activeSlotSize = currentTank.TankSO.ActiveItemInventorySize;
        //foreach (var active in _activeItemEquipmentDataDict._itemEquipmentList)
        //{
        //    if (idx - 2 > activeSlotSize)
        //    {
        //        _gearImages[idx].sprite = null;
        //        _gearImages[idx].gameObject.SetActive(false);
        //        _lockImages[idx++].SetActive(true);
        //        continue;
        //    }

        //    _lockImages[idx].SetActive(false);
        //    if (active == "")
        //    {
        //        _gearImages[idx].sprite = null;
        //        _gearImages[idx++].gameObject.SetActive(false);
        //        continue;
        //    }

        //    Item_Base itemInfo = AddressablesManager.Instance.GetResource<GameObject>(active).GetComponent<Item_Base>();
        //    _gearImages[idx].sprite = itemInfo.ItemSO.Image;
        //    _gearImages[idx++].gameObject.SetActive(true);
        //}

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

        foreach (var id in ussrData._tankProgressList)
        {
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.GetComponent<Image>().sprite = _flagImageDict[CountryType.USSR];
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                FindObjectOfType<TankModelManager>().ChangeTankModel(AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>());
            });
        }
    }

    public void OnStartButton()
    {
        if (ShellEmptyCheck())
        {
            WarningShellEmpty();
            return;
        }

        _startButton.interactable = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
        Pool.PoolManager.DeleteAllPool();
        EventManager.ClearEvent();
    }

    public void OnTrainingStart()
    {
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
            _warningText.SetText("총알이 장착되어 있지 않습니다!\n총알을 장착해주세요.");
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
    }

    public void OnSettingButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Setting, CanvasType);
    }

    public void OnShopButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.Shop, CanvasType);
    }

    public void OnTechTreeButton()
    {
        CanvasManager.ChangeCanvas(CanvasType.TechTree, CanvasType);
    }

    public void OnOpenItem()
    {
        CanvasManager.ChangeCanvas(CanvasType.Gear, CanvasType);
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
    }
}
