using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsTexts : MonoBehaviour
{
    [SerializeField]
    private Text _freeGoodsText = null;

    [SerializeField]
    private Text _paidGoodsText = null;

    public void SetGoodsTexts(int freeGoods, int paidGoods)
    {
        _freeGoodsText.text = freeGoods.ToString();
        _paidGoodsText.text = paidGoods.ToString();
    }
}
