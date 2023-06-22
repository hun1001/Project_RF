using UnityEngine;
using TMPro;

public class GoodsTexts : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _freeGoodsText = null;

    [SerializeField]
    private TextMeshProUGUI _paidGoodsText = null;

    public void SetGoodsTexts(int freeGoods, int paidGoods)
    {
        _freeGoodsText.text = freeGoods.ToString();
        _paidGoodsText.text = paidGoods.ToString();
    }
}
