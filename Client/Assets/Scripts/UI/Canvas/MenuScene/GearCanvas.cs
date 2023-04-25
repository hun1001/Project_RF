using DG.Tweening;
using Item;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GearCanvas : BaseCanvas
{
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
    private RectTransform _inventoryTransform;

    [Space(10f)]
    [SerializeField]
    private Image[] _passiveItemImages;
    [SerializeField]
    private Toggle[] _passiveItemToggles;

    [Space(20f)]
    [SerializeField]
    private Image[] _activeItemImages;
    [SerializeField]
    private Toggle[] _activeItemToggles;

    [Space(20f)]
    [SerializeField]
    private Image _shellImage;
    [SerializeField]
    private Toggle _shellToggle;

    private Dictionary<int, GameObject> _passiveEquipItemDictionary = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _activeEquipItemDictionary = new Dictionary<int, GameObject>();

    int a = 0;
    int b = 0;
    private void Awake()
    {
        foreach (Item_Base itemInfo in _itemList.PassiveItemList)
        {
            var item = Instantiate(_itemTemplate, _itemContent);
            item.SetActive(true);
            item.GetComponent<Image>().sprite = itemInfo.ItemSO.Image;
            _itemDictionary.Add(itemInfo, item);
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (itemInfo.ItemSO.ItemType == ItemType.Passive)
                {
                    if(_passiveEquipItemDictionary.ContainsKey(a) == false)
                    {
                        _passiveEquipItemDictionary.Add(a, item);
                        _passiveItemImages[a].gameObject.SetActive(true);
                    }
                    else
                    {
                        _passiveEquipItemDictionary[a].SetActive(true);
                        _passiveEquipItemDictionary[a] = item;
                    }
                    _passiveItemImages[a].sprite = itemInfo.ItemSO.Image;
                }

                else if (itemInfo.ItemSO.ItemType == ItemType.Active)
                {
                    if (_activeEquipItemDictionary.ContainsKey(b) == false)
                    {
                        _activeEquipItemDictionary.Add(b, item);
                        _activeItemImages[b].gameObject.SetActive(true);
                    }
                    else
                    {
                        _activeEquipItemDictionary[b].SetActive(true);
                        _activeEquipItemDictionary[b] = item;
                    }
                    _activeItemImages[b].sprite = itemInfo.ItemSO.Image;
                }

                item.SetActive(false);
                CloseInvetory();
            });
        }
    }

    public void OnPassiveInventory(int idx)
    {
        foreach(var item in _itemDictionary)
        {
            if(item.Key.ItemSO.ItemType == ItemType.Passive)
            {
                if (_passiveEquipItemDictionary.ContainsValue(item.Value)) continue;
                item.Value.SetActive(true);
            }
            else if (item.Key.ItemSO.ItemType != ItemType.Passive)
            {
                item.Value.SetActive(false);
            }
        }
        _inventoryTransform.DOAnchorPosY(0f, 0.7f);
        _passiveItemToggles[idx].isOn = true;
        a = idx;
    }

    public void OnActiveInventory(int idx)
    {
        foreach (var item in _itemDictionary)
        {
            if (item.Key.ItemSO.ItemType == ItemType.Active)
            {
                if (_passiveEquipItemDictionary.ContainsValue(item.Value)) continue;
                item.Value.SetActive(true);
            }
            else if (item.Key.ItemSO.ItemType != ItemType.Active)
            {
                item.Value.SetActive(false);
            }
        }
        _inventoryTransform.DOAnchorPosY(0f, 0.7f);
        _activeItemToggles[idx].isOn = true;
        b = idx;
    }

    public void OnShellInventory()
    {
        _inventoryTransform.DOAnchorPosY(0f, 0.7f);
        _shellToggle.isOn = true;
    }

    public void CloseInvetory()
    {
        foreach(var toggle in _passiveItemToggles)
        {
            toggle.isOn = false;
        }
        foreach (var toggle in _activeItemToggles)
        {
            toggle.isOn = false;
        }
        _shellToggle.isOn = false;

        _inventoryTransform.DOAnchorPosY(-_inventoryTransform.sizeDelta.y, 0.7f);
    }
}
