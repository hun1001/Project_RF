using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public static class GoodsManager
{
    private static GoodsInformation _goodsInformation = new GoodsInformation();

    private static Action<int> _onGoodsChanged;
    public static Action<int> OnGoodsChanged
    {
        set => _onGoodsChanged = value;
    }

    private static GoodsInformation GetGoodsInformation()
    {
        _goodsInformation = SaveManager.Load<GoodsInformation>(SaveKey.GoodsInformation);
        return _goodsInformation;
    }

    private static void SetGoodsInformation(GoodsInformation goodsInformation)
    {
        _goodsInformation = goodsInformation;
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
    /// Free Goods Increase
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

    // /// <summary> 무료 재화 </summary>
    // private static int _freeGoods;
    // /// <summary> 유료 재화 </summary>
    // private static int _paidGoods;
    // /// <summary> 인게임 재화 </summary>
    // private static int _gameGoods;
    // /// <summary> 자유 경험치 </summary>
    // private static int _freeExperience;
    // /// <summary> 특수 재화 </summary>
    // private static int _specialGoods;

    // /// <summary> 해당 재화를 반환하는 함수 </summary>
    // /// <param name="goodsType"> 재화 타입 </param>
    // /// <returns> 해당 재화의 값 </returns>
    // public static int GetGoods(GoodsType goodsType) => goodsType switch
    // {
    //     GoodsType.FreeGoods => _freeGoods,
    //     GoodsType.PaidGoods => _paidGoods,
    //     // GoodsType.GameGoods => _gameGoods,
    //     // GoodsType.FreeExperience => _freeExperience,
    //     // GoodsType.SpecialGoods => _specialGoods,
    //     _ => -1
    // };

    // /// <summary> 해당 재화의 값을 설정하는 함수 </summary>
    // /// <param name="goodsType"> 재화 타입 </param>
    // /// <param name="value"> 설정하는 값 </param>
    // /// <returns> 해당 재화가 설정된 값 </returns>
    // public static int SetGoods(GoodsType goodsType, int value) => goodsType switch
    // {
    //     GoodsType.FreeGoods => _freeGoods = value,
    //     GoodsType.PaidGoods => _paidGoods = value,
    //     // GoodsType.GameGoods => _gameGoods = value,
    //     // GoodsType.FreeExperience => _freeExperience = value,
    //     // GoodsType.SpecialGoods => _specialGoods = value,
    //     _ => -1
    // };

    // /// <summary> 해당 재화에 값만큼 추가하는 함수 </summary>
    // /// <param name="goodsType"> 재화 타입 </param>
    // /// <param name="value"> 추가되는 값 </param>
    // /// <returns> 해당 재화가 추가된 값 </returns>
    // public static int AddGoods(GoodsType goodsType, int value) => goodsType switch
    // {
    //     GoodsType.FreeGoods => _freeGoods += value,
    //     GoodsType.PaidGoods => _paidGoods += value,
    //     // GoodsType.GameGoods => _gameGoods += value,
    //     // GoodsType.FreeExperience => _freeExperience += value,
    //     // GoodsType.SpecialGoods => _specialGoods += value,
    //     _ => -1
    // };

    // /// <summary> 해당 재화에 값만큼 감소시키는 함수 </summary>
    // /// <param name="goodsType"> 재화 타입 </param>
    // /// <param name="value"> 감소하려는 값 </param>
    // /// <returns> 해당 재화를 감소시켰을때 0 미만인가 - bool </returns>
    // public static bool DecreaseGoods(GoodsType goodsType, int value)
    // {
    //     switch (goodsType)
    //     {
    //         case GoodsType.FreeGoods:
    //             {
    //                 if (_goodsInformation.freeGoods - value >= 0)
    //                 {
    //                     _goodsInformation.freeGoods -= value;
    //                     return true;
    //                 }
    //             }
    //             break;
    //         case GoodsType.PaidGoods:
    //             {
    //                 if (_goodsInformation.paidGoods - value >= 0)
    //                 {
    //                     _goodsInformation.paidGoods -= value;
    //                     return true;
    //                 }
    //             }
    //             break;
    //         // case GoodsType.GameGoods:
    //         //     {
    //         //         if (_gameGoods - value >= 0)
    //         //         {
    //         //             _gameGoods -= value;
    //         //             return true;
    //         //         }
    //         //     }
    //         //     break;
    //         // case GoodsType.FreeExperience:
    //         //     {
    //         //         if (_freeExperience - value >= 0)
    //         //         {
    //         //             _freeExperience -= value;
    //         //             return true;
    //         //         }
    //         //     }
    //         //     break;
    //         // case GoodsType.SpecialGoods:
    //         //     {
    //         //         if (_specialGoods - value >= 0)
    //         //         {
    //         //             _specialGoods -= value;
    //         //             return true;
    //         //         }
    //         //     }
    //         //     break;
    //         default:
    //             break;
    //     }
    //     return false;
    // }
}
