using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public static class GoodsManager
{
    private static GoodsInformation _goodsInformation = new GoodsInformation();

    private static Action<int, int> _onGoodsChanged;
    public static void AddOnGoodsChanged(Action<int, int> onGoodsChanged)
    {
        _onGoodsChanged += onGoodsChanged;
    }

    private static GoodsInformation GetGoodsInformation()
    {
        _goodsInformation = SaveManager.Load<GoodsInformation>(SaveKey.GoodsInformation);
        return _goodsInformation;
    }

    private static void SetGoodsInformation(GoodsInformation goodsInformation)
    {
        _goodsInformation = goodsInformation;
        _onGoodsChanged?.Invoke(_goodsInformation.freeGoods, _goodsInformation.paidGoods);
        SaveManager.Save(SaveKey.GoodsInformation, _goodsInformation);
    }

    public static int FreeGoods => GetGoodsInformation().freeGoods;
    public static int PaidGoods => GetGoodsInformation().paidGoods;

    public static void IncreaseFreeGoods(int value)
    {
        GoodsInformation goodsInformation = GetGoodsInformation();
        goodsInformation.freeGoods += value;
        SetGoodsInformation(goodsInformation);
    }

    public static void IncreasePaidGoods(int value)
    {
        GoodsInformation goodsInformation = GetGoodsInformation();
        goodsInformation.paidGoods += value;
        SetGoodsInformation(goodsInformation);
    }

    /// <summary>
    /// Free Goods Decrease
    /// </summary>
    /// <param name="value">Decrease value</param>
    /// <returns>FreeGoods < value</returns>
    public static bool DecreaseFreeGoods(int value)
    {
        if (FreeGoods < value)
        {
            return false;
        }
        else
        {
            GoodsInformation goodsInformation = GetGoodsInformation();
            goodsInformation.freeGoods -= value;
            SetGoodsInformation(goodsInformation);
            return true;
        }
    }

    /// <summary>
    /// Paid Goods Decrease
    /// </summary>
    /// <param name="value">Decrease value</param>
    /// <returns>PaidGoods < value</returns>
    public static bool DecreasePaidGoods(int value)
    {
        if (PaidGoods < value)
        {
            return false;
        }
        else
        {
            GoodsInformation goodsInformation = GetGoodsInformation();
            goodsInformation.paidGoods -= value;
            SetGoodsInformation(goodsInformation);
            return true;
        }
    }
}
