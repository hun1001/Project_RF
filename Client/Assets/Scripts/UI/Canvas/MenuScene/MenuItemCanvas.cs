using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemCanvas : BaseCanvas
{
    [Header("Tank")]
    [SerializeField]
    private Dropdown _haveTankList;

    [Header("ItemList")]
    [SerializeField]
    private ItemListSO _itemList;
    [SerializeField]
    private GameObject _itemTemplate;
    [SerializeField]
    private RectTransform _itemContent;
    private Dictionary<Item_Base, GameObject> _itemDictionary = new Dictionary<Item_Base, GameObject>();

    [Header("Equipped Item")]
    [SerializeField]
    private RectTransform _buttonsParent;
    [SerializeField]
    private Image[] _selectItems;
    private Button[] _equippedButtons;

    private enum ItemType : int
    {
        Commander = 0,
        Connoneer = 1,
        Loader = 2,
        Driver = 3,
    }
    int a = 0;
    private void Awake()
    {
        _equippedButtons = _buttonsParent.GetComponentsInChildren<Button>();
        foreach(Item_Base itemInfo in _itemList.ItemList)
        {
            var item = Instantiate(_itemTemplate, _itemContent);
            item.SetActive(true);
            item.GetComponent<Image>().sprite = itemInfo.ItemSO.Image;
            _itemDictionary.Add(itemInfo, item);
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                _selectItems[a].sprite = itemInfo.ItemSO.Image;
                _selectItems[a++].gameObject.SetActive(true);
                item.SetActive(false);
            });
        }
    }
}
