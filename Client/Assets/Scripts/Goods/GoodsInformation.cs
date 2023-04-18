using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct GoodsInformation
{
    public int freeGoods;
    public int paidGoods;

    public void SetGoodsInformation(GoodsInformation goodsInformation)
    {
        this.freeGoods = goodsInformation.freeGoods;
        this.paidGoods = goodsInformation.paidGoods;
    }
}
