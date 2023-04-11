using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ShopToggle
{
    PremiumTank,
    Item,
    RandomGacha,
    PaidGoods,
}

public class ShopCanvas : BaseCanvas
{
    [Header("Scroll Rect")]
    [SerializeField]
    private ScrollRect _scrollRect;

    [Header("Toggle")]
    [SerializeField]
    private Toggle _premiumTankToggle;
    [SerializeField]
    private Toggle _itemToggle;
    [SerializeField]
    private Toggle _randomGachaToggle;
    [SerializeField]
    private Toggle _paidGoodsToggle;

    [Header("Animation")]
    [SerializeField]
    private RectTransform _content;
    [SerializeField]
    private RectTransform _toggleGroup;
    private Image[] _toggleImages;

    private void Awake()
    {
        _premiumTankToggle.onValueChanged.AddListener(delegate { OnToggle(ShopToggle.PremiumTank); });
        _itemToggle.onValueChanged.AddListener(delegate { OnToggle(ShopToggle.Item); });
        _randomGachaToggle.onValueChanged.AddListener(delegate { OnToggle(ShopToggle.RandomGacha); });
        _paidGoodsToggle.onValueChanged.AddListener(delegate { OnToggle(ShopToggle.PaidGoods); });

        _toggleImages = _toggleGroup.GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        _scrollRect.normalizedPosition = Vector2.zero;

        _startSequence = DOTween.Sequence()
        .SetAutoKill(false)
        .PrependCallback(() =>
        {
            _content.anchoredPosition += Vector2.down * 300f;
            for(int i = 0; i < _toggleImages.Length; i += 2)
            {
                _toggleImages[i].DOFade(0f, 0f);
            }
        })
        .Append(_content.DOAnchorPosY(0f, 1f))
        .Append(_toggleImages[0].DOFade(1f, 0.5f))
        .Insert(1.25f, _toggleImages[2].DOFade(1f, 0.5f))
        .Insert(1.5f, _toggleImages[4].DOFade(1f, 0.5f))
        .Insert(1.75f, _toggleImages[6].DOFade(1f, 0.5f));
    }

    // 0 ~ 1 값이 전달된다.
    public void OnScroll(Vector2 vector2)
    {
        float x = (float)Math.Round(vector2.x, 1);

        switch (x)
        {
            case 0f:
                {
                    _premiumTankToggle.isOn = true;
                }
                break;
            case 0.3f:
                {
                    _itemToggle.isOn = true;
                }
                break;
            case 0.7f:
                {
                    _randomGachaToggle.isOn = true;
                }
                break;
            case 1f:
                {
                    _paidGoodsToggle.isOn = true;
                }
                break;
        }
    }

    public void OnToggle(ShopToggle value)
    {
        switch (value)
        {
            case ShopToggle.PremiumTank:
                {
                    _scrollRect.normalizedPosition = Vector2.zero;
                }
                break;
            case ShopToggle.Item:
                {
                    _scrollRect.normalizedPosition = new Vector2(0.3f, 0f);
                }
                break;
            case ShopToggle.RandomGacha:
                {
                    _scrollRect.normalizedPosition = new Vector2(0.7f, 0f);
                }
                break;
            case ShopToggle.PaidGoods:
                {
                    _scrollRect.normalizedPosition = Vector2.right;
                }
                break;
            default: break;
        }
    }

    public override void OnHomeButton()
    {
        base.OnHomeButton();
        _scrollRect.normalizedPosition = Vector2.zero;
    }
}
