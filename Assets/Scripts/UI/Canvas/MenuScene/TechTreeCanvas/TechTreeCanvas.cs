using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using Addressable;

public class TechTreeCanvas : BaseCanvas
{
    [Header("Goods")]
    [SerializeField]
    private GoodsTexts _goodsTexts = null;

    [Header("Tier")]
    [SerializeField]
    private TechTreeTierLine _tankTierLine = null;

    [Header("TechTree")]
    [SerializeField]
    private TechTreeCountryToggles _countryToggleGroup = null;

    [SerializeField]
    private RectTransform _tankNodeContentTransform = null;

    [SerializeField]
    private GameObject _tankNodeTemplate = null;

    [SerializeField]
    private RectTransform _techTreeScrollView = null;
    private ScrollRect _scrollRect = null;

    [SerializeField]
    private GameObject _tankInformation = null;
    [SerializeField]
    private GameObject _tankInformationPanel = null;

    [Header("Resource")]
    [SerializeField]
    private TechTreeResourceSO _techTreeResourceSO = null;
    public TechTreeResourceSO TechTreeResourceSO => _techTreeResourceSO;

    private bool _isLeftClick = false;
    private bool _isRightClick = false;

    private void Awake()
    {
        _tankInformation.SetActive(false);
        _tankInformation.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            _tankInformation.SetActive(false);
            PlayButtonSound();
        });

        _goodsTexts.SetGoodsTexts(GoodsManager.FreeGoods, GoodsManager.PaidGoods);

        _tankNodeTemplate.SetActive(false);

        _techTreeScrollView.TryGetComponent(out _scrollRect);

        for (int i = 0; i < TechTreeInformationManager.TechTreeInformationList.Count; ++i)
        {
            int index = i;

            _countryToggleGroup.CreateCountryToggles(GetCountryFlagSprite(TechTreeInformationManager.TechTreeInformationList[i].Country), () =>
            {
                _tankInformation.SetActive(false);
                SetTechTreeInformation(index);
                PlayButtonSound();
            });

            if (i == 0)
            {
                _tankInformation.SetActive(false);
                SetTechTreeInformation(index);
                PlayButtonSound();
            }
        }
    }

    private void Start()
    {
        AddInputAction();
    }

    private void Update()
    {
        if (_isLeftClick)
        {
            _scrollRect.horizontalNormalizedPosition -= 0.01f;
        }
        if (_isRightClick)
        {
            _scrollRect.horizontalNormalizedPosition += 0.01f;
        }
    }

    protected override void AddInputAction()
    {
        KeyboardManager.Instance.AddKeyDownAction(KeyCode.LeftArrow, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
            {
                _isLeftClick = true;
            }
        });
        KeyboardManager.Instance.AddKeyUpAction(KeyCode.LeftArrow, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
            {
                _isLeftClick = false;
            }
        });

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.RightArrow, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
            {
                _isRightClick = true;
            }
        });
        KeyboardManager.Instance.AddKeyUpAction(KeyCode.RightArrow, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
            {
                _isRightClick = false;
            }
        });

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.A, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
            {
                _isLeftClick = true;
            }
        });
        KeyboardManager.Instance.AddKeyUpAction(KeyCode.A, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
            {
                _isLeftClick = false;
            }
        });

        KeyboardManager.Instance.AddKeyDownAction(KeyCode.D, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
            {
                _isRightClick = true;
            }
        });
        KeyboardManager.Instance.AddKeyUpAction(KeyCode.D, () =>
        {
            if (CanvasManager.ActiveCanvas == CanvasType)
            {
                _isRightClick = false;
            }
        });

        Action[] actions = new Action[TechTreeInformationManager.TechTreeInformationList.Count];
        for (int i = 0; i < TechTreeInformationManager.TechTreeInformationList.Count; ++i)
        {
            int idx = i;
            actions[idx] = () =>
            {
                if (CanvasManager.ActiveCanvas == CanvasType)
                {
                    _tankInformation.SetActive(false);
                    _countryToggleGroup.ChangeToggleValue(idx);
                }
            };
        }

        KeyboardManager.Instance.AddKeyDownActionList(actions);
    }

    private void SetTechTreeInformation(int index)
    {
        for (int i = 0; i < TechTreeInformationManager.TechTreeInformationList[index].techTreeList.Count; ++i)
        {
            TechTree techTree = TechTreeInformationManager.TechTreeInformationList[index].techTreeList[i];

            int maxTier = techTree.GetMaxTier();
            int width = techTree.GetWidth();

            ResetTankNode();
            _tankTierLine.ResetTierLine();

            _tankTierLine.SetTierLine(maxTier);

            TechTreeCanvasBFSIterator iterator = new TechTreeCanvasBFSIterator(techTree, _tankNodeTemplate.transform.position);

            while (iterator.IsSearching)
            {
                var tankNode = iterator.GetNextNode();
                var tankNodePosition = iterator.GetNextPosition();
                int tierIndex = iterator.GetNextTier();

                var tankNodeUI = Instantiate(_tankNodeTemplate, _tankNodeContentTransform).GetComponent<TankNode>();
                tankNodeUI.transform.position = tankNodePosition;

                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(tankNode.tankAddress).GetComponent<Tank>();

                tankNodeUI.SetTankNode(GetTankTypeSprite(tank.TankSO.TankType), TechTreeResourceSO.TankTierNumber[tierIndex], tank.ID, () =>
                {
                    PlayButtonSound();

                    _tankInformation.SetActive(true);
                    var topUI = _tankInformationPanel.transform.GetChild(0);
                    topUI.GetChild(1).GetComponent<Image>().sprite = GetCountryFlagSprite(TechTreeInformationManager.TechTreeInformationList[index].Country);
                    topUI.GetChild(2).GetComponent<Image>().sprite = GetTankTypeSprite(tank.TankSO.TankType);
                    topUI.GetChild(3).GetComponent<TextMeshProUGUI>().text = TechTreeResourceSO.TankTierNumber[tierIndex];
                    topUI.GetChild(4).GetComponent<TextMeshProUGUI>().text = tank.ID;

                    _tankInformationPanel.transform.GetChild(1).GetComponent<Image>().sprite = null;

                    var stats = _tankInformationPanel.transform.GetChild(2);
                    // Health
                    float health = 1000f * ((tank.TankSO.HP * 0.1f) * (tank.TankSO.Armour * 0.1f)) / 11392f;
                    stats.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = health * 0.001f;
                    stats.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", health);
                    // Power
                    float power = 1000f * (tank.GetComponent<Turret>().TurretSO.AtkPower * tank.GetComponent<Turret>().TurretSO.PenetrationPower) / 11440f;
                    stats.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = power * 0.001f;
                    stats.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", power);
                    // Movement
                    float movement = 1000f * ((tank.TankSO.MaxSpeed * 0.4f) * (tank.TankSO.Acceleration * 0.2f) * (tank.TankSO.RotationSpeed * 0.2f) * (tank.GetComponent<Turret>().TurretSO.RotationSpeed * 0.2f)) / 93177f;
                    stats.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = movement * 0.001f;
                    stats.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", movement);

                    _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
                    _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        PlayButtonSound();

                        Debug.Log("구매 제거됨. 이거 대체 필요");
                    });
                });

                tankNodeUI.SetConnectLine(tankNode.upChild != null, tankNode.child != null, tankNode.downChild != null);

                tankNodeUI.SetActive(true);
            }
        }
    }

    private void ResetTankNode()
    {
        for (int k = 2; k < _tankNodeContentTransform.transform.childCount; ++k)
        {
            Destroy(_tankNodeContentTransform.transform.GetChild(k).gameObject);
        }
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        _startSequence = DOTween.Sequence()
        .PrependCallback(() => _techTreeScrollView.anchoredPosition = Vector2.right * 1000f)
        .Append(_techTreeScrollView.DOAnchorPosX(0f, 1f))
        .AppendCallback(() => _scrollRect.normalizedPosition = Vector2.zero);
        _tankInformation.SetActive(false);
    }

    public override void OnBackButton()
    {
        _tankInformation.SetActive(false);
        base.OnBackButton();
    }

    public void DownLeftButton()
    {
        _isLeftClick = true;
    }

    public void UpLeftButton()
    {
        _isLeftClick = false;
    }

    public void DownRightButton()
    {
        _isRightClick = true;
    }

    public void UpRightButton()
    {
        _isRightClick = false;
    }

    public Sprite GetTankTypeSprite(TankType tankType)
    {
        Sprite sprite = null;
        switch (tankType)
        {
            case TankType.Light:
                sprite = TechTreeResourceSO.TankTypeSprites[0];
                break;
            case TankType.Medium:
                sprite = TechTreeResourceSO.TankTypeSprites[1];
                break;
            case TankType.Heavy:
                sprite = TechTreeResourceSO.TankTypeSprites[2];
                break;
            case TankType.Destroyer:
                sprite = TechTreeResourceSO.TankTypeSprites[3];
                break;
            default:
                Debug.LogError("TankType Error");
                break;
        }
        return sprite;
    }

    public Sprite GetCountryFlagSprite(CountryType country)
    {
        Sprite sprite = null;
        switch (country)
        {
            case CountryType.USSR:
                sprite = TechTreeResourceSO.CountryFlagSprites[0];
                break;
            case CountryType.Germany:
                sprite = TechTreeResourceSO.CountryFlagSprites[1];
                break;
            case CountryType.USA:
                sprite = TechTreeResourceSO.CountryFlagSprites[2];
                break;
            case CountryType.Britain:
                sprite = TechTreeResourceSO.CountryFlagSprites[3];
                break;
            case CountryType.France:
                sprite = TechTreeResourceSO.CountryFlagSprites[4];
                break;
            default:
                Debug.LogError("Country Error");
                break;
        }
        return sprite;  
    }
}
