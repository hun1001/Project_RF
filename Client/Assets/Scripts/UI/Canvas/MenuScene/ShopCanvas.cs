using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        _premiumTankToggle.onValueChanged.AddListener(delegate{ OnToggle(ShopToggle.PremiumTank); });
        _itemToggle.onValueChanged.AddListener(delegate { OnToggle(ShopToggle.Item); });
        _randomGachaToggle.onValueChanged.AddListener(delegate { OnToggle(ShopToggle.RandomGacha); });
        _paidGoodsToggle.onValueChanged.AddListener(delegate { OnToggle(ShopToggle.PaidGoods); });
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
            case 0.8f:
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
        switch(value)
        {
            case ShopToggle.PremiumTank:
                {
                    _scrollRect.horizontalNormalizedPosition = 0f;
                }
                break;
            case ShopToggle.Item:
                {
                    _scrollRect.horizontalNormalizedPosition = 0.3f;
                }
                break;
            case ShopToggle.RandomGacha:
                {
                    _scrollRect.horizontalNormalizedPosition = 0.8f;
                }
                break;
            case ShopToggle.PaidGoods:
                {
                    _scrollRect.horizontalNormalizedPosition = 1f;
                }
                break;
            default : break;
        }
    }

    public override void OnHomeButton()
    {
        base.OnHomeButton();
        _scrollRect.horizontalNormalizedPosition = 0f;
    }
}
