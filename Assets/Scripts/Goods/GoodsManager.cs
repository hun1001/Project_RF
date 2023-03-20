using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public static class GoodsManager
{
    private static int _freeGoods;
    private static int _paidGoods;
    private static int _gameGoods;
    private static int _freeExperience;
    private static int _specialGoods;

    public static int GetGoods(GoodsEnum goodsType) => goodsType switch
    {
        GoodsEnum.FreeGoods => _freeGoods,
        GoodsEnum.PaidGoods => _paidGoods,
        GoodsEnum.GameGoods => _gameGoods,
        GoodsEnum.FreeExperience => _freeExperience,
        GoodsEnum.SpecialGoods => _specialGoods,
    };
}
