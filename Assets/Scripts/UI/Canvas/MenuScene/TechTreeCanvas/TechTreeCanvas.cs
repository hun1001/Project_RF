using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

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
    private RectTransform _techTreeScrollView = null;
    private ScrollRect _scrollRect = null;

    [SerializeField]
    private GameObject _tankInformation = null;
    [SerializeField]
    private GameObject _tankInformationPanel = null;

    [Header("TechTree Data")]
    [SerializeField]
    private TechTreeSO[] _techTreeSO = null;
    public TechTreeSO[] TechTreeSO => _techTreeSO;

    [SerializeField]
    private Sprite[] _tankTypeSprites = null;

    public readonly string[] TankTierNumber = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX" };

    private GameObject _tankNodeRowTemplate = null;
    private GameObject _tankNodeTemplate = null;
    private GameObject _tankNodeConnectHorizontalLineTemplate = null;
    private GameObject _tankNodeNullTemplate = null;
    private GameObject _tankNodeConnectHorizontalNullLineTemplate = null;

    private bool _isLeftClick = false;
    private bool _isRightClick = false;

    private void Awake()
    {
        if (TechTreeDataManager.HasTank(CountryType.USSR, "T-34") == false)
        {
            TechTreeDataManager.AddTank(CountryType.USSR, "T-34");
        }

        _tankInformation.SetActive(false);
        _tankInformation.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            _tankInformation.SetActive(false);
            PlayButtonSound();
        });

        _goodsTexts.SetGoodsTexts(GoodsManager.FreeGoods, GoodsManager.PaidGoods);


        _tankNodeRowTemplate = _tankNodeContentTransform.GetChild(0).gameObject;
        _tankNodeTemplate = _tankNodeRowTemplate.transform.GetChild(0).gameObject;
        _tankNodeConnectHorizontalLineTemplate = _tankNodeRowTemplate.transform.GetChild(1).gameObject;
        _tankNodeNullTemplate = _tankNodeRowTemplate.transform.GetChild(2).gameObject;
        _tankNodeConnectHorizontalNullLineTemplate = _tankNodeRowTemplate.transform.GetChild(3).gameObject;

        _tankNodeRowTemplate.SetActive(false);
        _tankNodeTemplate.SetActive(false);
        _tankNodeConnectHorizontalLineTemplate.SetActive(false);
        _tankNodeNullTemplate.SetActive(false);
        _tankNodeConnectHorizontalNullLineTemplate.SetActive(false);

        _techTreeScrollView.TryGetComponent(out _scrollRect);

        for (int i = 0; i < TechTreeSO.Length; ++i)
        {
            int index = i;

            _countryToggleGroup.CreateCountryToggles(TechTreeSO[index].FlagSprite, () =>
            {
                _tankInformation.SetActive(false);
                SetTechTree(index);
            });
        }

        _countryToggleGroup.ChangeFirstToggleValue(true);

        for (int i = 0; i < TechTreeSO.Length; i++)
        {
            int index = i;
            _countryToggleGroup.AddCountryToggleAction(index, () =>
            {
                PlayButtonSound();
            });
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

        Action[] actions = new Action[TechTreeSO.Length];
        for (int i = 0; i < TechTreeSO.Length; ++i)
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

    private void SetTechTree(int index)
    {
        ResetTankNode();
        _tankTierLine.ResetTierLine();

        _tankTierLine.SetTierLine(TechTreeSO[index].GetMaximumLength());

        for (int j = 0; j < TechTreeSO[index].Length; ++j)
        {
            int jIndex = j;
            var rowTransform = Instantiate(_tankNodeRowTemplate, _tankNodeContentTransform).transform;

            for (int l = 0; l < TechTreeSO[index].GetTankArrayLength(jIndex); ++l)
            {
                int lIndex = l;

                GameObject node;

                if (TechTreeSO[index][jIndex, lIndex] == null)
                {
                    node = Instantiate(_tankNodeNullTemplate, rowTransform);
                    if (lIndex != TechTreeSO[index].GetTankArrayLength(jIndex) - 1)
                    {
                        if (TechTreeSO[index][jIndex, lIndex + 1] == null)
                        {
                            var tankNodeConnectLine = Instantiate(_tankNodeConnectHorizontalNullLineTemplate, rowTransform);
                            tankNodeConnectLine.SetActive(true);
                        }
                        else
                        {
                            var tankNodeConnectLine = Instantiate(_tankNodeConnectHorizontalLineTemplate, rowTransform);
                            tankNodeConnectLine.SetActive(true);
                        }
                    }
                }
                else
                {
                    node = Instantiate(_tankNodeTemplate, rowTransform);

                    var tNC = node.GetComponent<TankNode>();

                    bool isLock = !TechTreeDataManager.GetTechTreeProgress(TechTreeSO[index].CountryType)._tankProgressList.Contains(TechTreeSO[index][jIndex, lIndex].ID);

                    tNC.SetTankNode(GetTankTypeSprite(TechTreeSO[index][jIndex, lIndex].TankSO.TankType), TankTierNumber[lIndex], TechTreeSO[index][jIndex, lIndex].ID, isLock, () =>
                    {   
                        PlayButtonSound();

                        _tankInformation.SetActive(true);
                        var topUI = _tankInformationPanel.transform.GetChild(0);
                        topUI.GetChild(1).GetComponent<Image>().sprite = TechTreeSO[index].FlagSprite;
                        topUI.GetChild(2).GetComponent<Image>().sprite = GetTankTypeSprite(TechTreeSO[index][jIndex, lIndex].TankSO.TankType);
                        topUI.GetChild(3).GetComponent<TextMeshProUGUI>().text = TankTierNumber[lIndex];
                        topUI.GetChild(4).GetComponent<TextMeshProUGUI>().text = TechTreeSO[index][jIndex, lIndex].ID;

                        // ?±ÌÅ¨ ?¥Î?ÏßÄ ?ÜÏúº?àÍπå ?ºÎã®  null
                        _tankInformationPanel.transform.GetChild(1).GetComponent<Image>().sprite = null;

                        var stats = _tankInformationPanel.transform.GetChild(2);
                        // Health
                        float health = 1000f * ((TechTreeSO[index][jIndex, lIndex].TankSO.HP * 0.1f) * (TechTreeSO[index][jIndex, lIndex].TankSO.Armour * 0.1f)) / 11392f;
                        stats.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = health * 0.001f;
                        stats.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", health);
                        // Power
                        float power = 1000f * (TechTreeSO[index][jIndex, lIndex].GetComponent<Turret>().TurretSO.AtkPower * TechTreeSO[index][jIndex, lIndex].GetComponent<Turret>().TurretSO.PenetrationPower) / 11440f;
                        stats.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = power * 0.001f;
                        stats.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", power);
                        // Movement
                        float movement = 1000f * ((TechTreeSO[index][jIndex, lIndex].TankSO.MaxSpeed * 0.4f) * (TechTreeSO[index][jIndex, lIndex].TankSO.Acceleration * 0.2f) * (TechTreeSO[index][jIndex, lIndex].TankSO.RotationSpeed * 0.2f) * (TechTreeSO[index][jIndex, lIndex].GetComponent<Turret>().TurretSO.RotationSpeed * 0.2f)) / 93177f;
                        stats.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = movement * 0.001f;
                        stats.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", movement);

                        _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
                        _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            PlayButtonSound();

                            if (GoodsManager.DecreaseFreeGoods((int)TechTreeSO[index][jIndex, lIndex].TankSO.Price))
                            {
                                TechTreeDataManager.AddTank(TechTreeSO[index].CountryType, TechTreeSO[index][jIndex, lIndex].ID);
                                tNC.IsTankLocked = false;
                                _tankInformation.SetActive(false);
                            }
                        });
                    });

                    node.GetComponent<Image>().enabled = true;

                    if (lIndex != TechTreeSO[index].GetTankArrayLength(jIndex) - 1)
                    {
                        var tankNodeConnectLine = Instantiate(_tankNodeConnectHorizontalLineTemplate, rowTransform);
                        tankNodeConnectLine.SetActive(true);
                    }
                }

                node.SetActive(true);
            }

            rowTransform.GetComponent<HorizontalLayoutGroup>().enabled = true;
            rowTransform.GetComponent<ContentSizeFitter>().enabled = true;
            rowTransform.gameObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)rowTransform.transform);
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
                sprite = _tankTypeSprites[0];
                break;
            case TankType.Medium:
                sprite = _tankTypeSprites[1];
                break;
            case TankType.Heavy:
                sprite = _tankTypeSprites[2];
                break;
            case TankType.Destroyer:
                sprite = _tankTypeSprites[3];
                break;
            default:
                Debug.LogError("TankType Error");
                break;
        }

        return sprite;
    }
}
