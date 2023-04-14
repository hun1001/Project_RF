using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemCanvas : BaseCanvas
{
    [Header("Item")]
    [SerializeField]
    private ItemListSO _itemList;
    [SerializeField]
    private GameObject _itemTemplate;
    [SerializeField]
    private RectTransform _itemContent;
    [SerializeField]
    private Image[] _selectItems;

    private void Awake()
    {
        foreach(Item_Base itemInfo in _itemList.ItemList)
        {
            var item = Instantiate(_itemTemplate, _itemContent);
            item.SetActive(true);
            item.GetComponent<Image>().sprite = itemInfo.ItemSO.Image;
            item.GetComponent<Button>().onClick.AddListener(() =>
            {

            });
        }
    }
}
