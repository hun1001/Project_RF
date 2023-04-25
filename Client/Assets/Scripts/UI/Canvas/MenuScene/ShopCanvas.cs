﻿using System;
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
    private ToggleGroup _toggleGroup;
    [SerializeField]
    private Toggle _premiumTankToggle;
    [SerializeField]
    private Toggle _itemToggle;
    [SerializeField]
    private Toggle _randomGachaToggle;
    [SerializeField]
    private Toggle _paidGoodsToggle;

    [Header("Button")]
    [SerializeField]
    private Button _premiumTankButton;
    [SerializeField]
    private Button _itemButton;
    [SerializeField]
    private Button _randomGachaButton;
    [SerializeField]
    private Button _paidGoodsButton;

    [Header("Animation")]
    [SerializeField]
    private RectTransform _content;
    private Image[] _toggleImages;

    [Header("Product")]
    [SerializeField]
    private GameObject _premiumTankTemplate;
    [SerializeField]
    private GameObject _itemTemplate;
    [SerializeField]
    private GameObject _paidGoodsTemplate;
    [SerializeField]
    private RectTransform[] _productContents;
    [SerializeField]
    private GameObject _productInformation;

    private void Awake()
    {
        _scrollRect.onValueChanged.AddListener(OnScroll);
        _premiumTankButton.onClick.AddListener(delegate { OnToggle(ShopToggle.PremiumTank); });
        _itemButton.onClick.AddListener(delegate { OnToggle(ShopToggle.Item); });
        _randomGachaButton.onClick.AddListener(delegate { OnToggle(ShopToggle.RandomGacha); });
        _paidGoodsButton.onClick.AddListener(delegate { OnToggle(ShopToggle.PaidGoods); });

        _toggleImages = _toggleGroup.GetComponentsInChildren<Image>();

        for(int i = 0; i < 6; i++)
        {
            var product = Instantiate(_premiumTankTemplate, _productContents[0]);
            product.SetActive(true);
        }

        for (int i = 0; i < 3; i++)
        {
            var product = Instantiate(_itemTemplate, _productContents[1]);
            product.SetActive(true);
        }

        for (int i = 0; i < 6; i++)
        {
            var product = Instantiate(_paidGoodsTemplate, _productContents[3]);
            product.SetActive(true);
        }
    }

    private void Start()
    {
        _scrollRect.normalizedPosition = Vector2.zero;

        _startSequence = DOTween.Sequence()
        .SetAutoKill(false)
        .PrependCallback(() =>
        {
            _scrollRect.onValueChanged.RemoveAllListeners();
            _content.anchoredPosition += Vector2.down * 300f;

            for (int i = 3; i < _toggleImages.Length; i += 3)
            {
                _toggleImages[i].DOFade(0f, 0f);
            }
        })
        .Insert(0f, _content.DOAnchorPosY(0f, 1f))
        .Insert(0.25f, _toggleImages[3].DOFade(1f, 1f))
        .Insert(0.5f, _toggleImages[6].DOFade(1f, 1f))
        .Insert(0.75f, _toggleImages[9].DOFade(1f, 1f))
        .AppendCallback(() => _scrollRect.onValueChanged.AddListener(OnScroll));
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
        _productInformation.SetActive(false);
    }
}
