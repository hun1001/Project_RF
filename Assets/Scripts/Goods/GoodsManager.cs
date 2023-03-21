using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public static class GoodsManager
{
    /// <summary> 무료 재화 </summary>
    private static int _freeGoods;
    /// <summary> 유료 재화 </summary>
    private static int _paidGoods;
    /// <summary> 인게임 재화 </summary>
    private static int _gameGoods;
    /// <summary> 자유 경험치 </summary>
    private static int _freeExperience;
    /// <summary> 특수 재화 </summary>
    private static int _specialGoods;

    /// <summary> 해당 재화를 반환하는 함수 </summary>
    /// <param name="goodsType"> 재화 타입 </param>
    /// <returns> 해당 재화의 값 </returns>
    public static int GetGoods(GoodsEnum goodsType) => goodsType switch
    {
        GoodsEnum.FreeGoods => _freeGoods,
        GoodsEnum.PaidGoods => _paidGoods,
        GoodsEnum.GameGoods => _gameGoods,
        GoodsEnum.FreeExperience => _freeExperience,
        GoodsEnum.SpecialGoods => _specialGoods,
        _ => -1
    };

    /// <summary> 해당 재화의 값을 설정하는 함수 </summary>
    /// <param name="goodsType"> 재화 타입 </param>
    /// <param name="value"> 설정하는 값 </param>
    /// <returns> 해당 재화가 설정된 값 </returns>
    public static int SetGoods(GoodsEnum goodsType, int value) => goodsType switch
    {
        GoodsEnum.FreeGoods => _freeGoods = value,
        GoodsEnum.PaidGoods => _paidGoods = value,
        GoodsEnum.GameGoods => _gameGoods = value,
        GoodsEnum.FreeExperience => _freeExperience = value,
        GoodsEnum.SpecialGoods => _specialGoods = value,
        _ => -1
    };

    /// <summary> 해당 재화에 값만큼 추가하는 함수 </summary>
    /// <param name="goodsType"> 재화 타입 </param>
    /// <param name="value"> 추가되는 값 </param>
    /// <returns> 해당 재화가 추가된 값 </returns>
    public static int AddGoods(GoodsEnum goodsType, int value) => goodsType switch
    {
        GoodsEnum.FreeGoods => _freeGoods += value,
        GoodsEnum.PaidGoods => _paidGoods += value,
        GoodsEnum.GameGoods => _gameGoods += value,
        GoodsEnum.FreeExperience => _freeExperience += value,
        GoodsEnum.SpecialGoods => _specialGoods += value,
        _ => -1
    };

    /// <summary> 해당 재화에 값만큼 감소시키는 함수 </summary>
    /// <param name="goodsType"> 재화 타입 </param>
    /// <param name="value"> 감소하려는 값 </param>
    /// <returns> 해당 재화를 감소시켰을때 0 미만인가 - bool </returns>
    public static bool DecreaseGoods(GoodsEnum goodsType, int value) 
    {
        switch(goodsType)
        {
            case GoodsEnum.FreeGoods:
                {
                    if(_freeGoods - value >= 0)
                    {
                        _freeGoods -= value;
                        return true;
                    }
                }
                break;
            case GoodsEnum.PaidGoods:
                {
                    if (_paidGoods - value >= 0)
                    {
                        _paidGoods -= value;
                        return true;
                    }
                }
                break;
            case GoodsEnum.GameGoods:
                {
                    if (_gameGoods - value >= 0)
                    {
                        _gameGoods -= value;
                        return true;
                    }
                }
                break;
            case GoodsEnum.FreeExperience:
                {
                    if (_freeExperience - value >= 0)
                    {
                        _freeExperience -= value;
                        return true;
                    }
                }
                break;
            case GoodsEnum.SpecialGoods:
                {
                    if (_specialGoods - value >= 0)
                    {
                        _specialGoods -= value;
                        return true;
                    }
                }
                break;
            default:
                break;
        }
        return false;
    }
}
