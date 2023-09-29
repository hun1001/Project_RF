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

    [Header("Resource")]
    [SerializeField]
    private TechTreeResourceSO _techTreeResourceSO = null;
    public TechTreeResourceSO TechTreeResourceSO => _techTreeResourceSO;

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

        for (int i = 0; i < TechTreeDataManager.TechTreeList.Count; ++i)
        {
            int index = i;

            //_countryToggleGroup.CreateCountryToggles(TechTreeSO[index].FlagSprite, () =>
            //{
            //    _tankInformation.SetActive(false);
            //    SetTechTree(index);
            //});
        }

        _countryToggleGroup.ChangeFirstToggleValue(true);

        for (int i = 0; i < TechTreeDataManager.TechTreeList.Count; i++)
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

        Action[] actions = new Action[TechTreeDataManager.TechTreeList.Count];
        for (int i = 0; i < TechTreeDataManager.TechTreeList.Count; ++i)
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
}
