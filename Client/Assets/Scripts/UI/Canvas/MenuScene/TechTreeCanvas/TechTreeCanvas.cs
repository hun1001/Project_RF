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
    private TechTree _techTree = null;

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

    private GameObject _tankNodeRowTemplate = null;
    private GameObject _tankNodeTemplate = null;
    private GameObject _tankNodeConnectHorizontalLineTemplate = null;
    private GameObject _tankNodeNullTemplate = null;
    private GameObject _tankNodeConnectHorizontalNullLineTemplate = null;

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

        for (int i = 0; i < _techTree.TechTreeSO.Length; ++i)
        {
            int index = i;

            _countryToggleGroup.CreateCountryToggles(_techTree.TechTreeSO[index].FlagSprite, () =>
            {
                _tankInformation.SetActive(false);
                SetTechTree(index);
            });
        }

        _countryToggleGroup.ChangeFirstToggleValue(true);

        for (int i = 0; i < _techTree.TechTreeSO.Length; i++)
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

        Action[] actions = new Action[_techTree.TechTreeSO.Length];
        for (int i = 0; i < _techTree.TechTreeSO.Length; ++i)
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

        _tankTierLine.SetTierLine(_techTree.TechTreeSO[index].GetMaximumLength());

        for (int j = 0; j < _techTree.TechTreeSO[index].Length; ++j)
        {
            int jIndex = j;
            var rowTransform = Instantiate(_tankNodeRowTemplate, _tankNodeContentTransform).transform;

            for (int l = 0; l < _techTree.TechTreeSO[index].GetTankArrayLength(jIndex); ++l)
            {
                int lIndex = l;

                GameObject node;

                if (_techTree.TechTreeSO[index][jIndex, lIndex] == null)
                {
                    node = Instantiate(_tankNodeNullTemplate, rowTransform);
                    if (lIndex != _techTree.TechTreeSO[index].GetTankArrayLength(jIndex) - 1)
                    {
                        if (_techTree.TechTreeSO[index][jIndex, lIndex + 1] == null)
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

                    bool isLock = !TechTreeDataManager.GetTechTreeProgress(_techTree.TechTreeSO[index].CountryType)._tankProgressList.Contains(_techTree.TechTreeSO[index][jIndex, lIndex].ID);

                    tNC.SetTankNode(_techTree.GetTankTypeSprite(_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.TankType), _techTree.TankTierNumber[lIndex], _techTree.TechTreeSO[index][jIndex, lIndex].ID, isLock, () =>
                    {
                        PlayButtonSound();

                        _tankInformation.SetActive(true);
                        var topUI = _tankInformationPanel.transform.GetChild(0);
                        topUI.GetChild(1).GetComponent<Image>().sprite = _techTree.TechTreeSO[index].FlagSprite;
                        topUI.GetChild(2).GetComponent<Image>().sprite = _techTree.GetTankTypeSprite(_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.TankType);
                        topUI.GetChild(3).GetComponent<TextMeshProUGUI>().text = _techTree.TankTierNumber[lIndex];
                        topUI.GetChild(4).GetComponent<TextMeshProUGUI>().text = _techTree.TechTreeSO[index][jIndex, lIndex].ID;

                        // ?�크 ?��?지 ?�으?�까 ?�단  null
                        _tankInformationPanel.transform.GetChild(1).GetComponent<Image>().sprite = null;

                        var stats = _tankInformationPanel.transform.GetChild(2);
                        // Health
                        float health = 1000f * ((_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.HP * 0.1f) * (_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.Armour * 0.1f)) / 11392f;
                        stats.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = health * 0.001f;
                        stats.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", health);
                        // Power
                        float power = 1000f * (_techTree.TechTreeSO[index][jIndex, lIndex].GetComponent<Turret>().TurretSO.AtkPower * _techTree.TechTreeSO[index][jIndex, lIndex].GetComponent<Turret>().TurretSO.PenetrationPower) / 11440f;
                        stats.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = power * 0.001f;
                        stats.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", power);
                        // Movement
                        float movement = 1000f * ((_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.MaxSpeed * 0.4f) * (_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.Acceleration * 0.2f) * (_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.RotationSpeed * 0.2f) * (_techTree.TechTreeSO[index][jIndex, lIndex].GetComponent<Turret>().TurretSO.RotationSpeed * 0.2f)) / 93177f;
                        stats.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = movement * 0.001f;
                        stats.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}", movement);

                        _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
                        _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            PlayButtonSound();

                            if (GoodsManager.DecreaseFreeGoods((int)_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.Price))
                            {
                                TechTreeDataManager.AddTank(_techTree.TechTreeSO[index].CountryType, _techTree.TechTreeSO[index][jIndex, lIndex].ID);
                                tNC.IsTankLocked = false;
                                _tankInformation.SetActive(false);
                            }
                        });
                    });

                    node.GetComponent<Image>().enabled = true;

                    if (lIndex != _techTree.TechTreeSO[index].GetTankArrayLength(jIndex) - 1)
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
}
