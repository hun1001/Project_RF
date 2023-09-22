using UnityEngine;
using TMPro;

public class GoodsTexts : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _freeGoodsText = null;

    [SerializeField]
    private TMP_Text _paidGoodsText = null;

    public void SetGoodsTexts(int freeGoods, int paidGoods)
    {
        _freeGoodsText.text = freeGoods.ToString();
        _paidGoodsText.text = paidGoods.ToString();
    }
}
