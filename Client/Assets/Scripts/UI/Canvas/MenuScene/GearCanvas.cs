using Item;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GearCanvas : BaseCanvas
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
    private Image[] _passiveItemImages;
    [SerializeField]
    private Image[] _activeItemImages;

    private Dictionary<int, GameObject> _passiveEquipItemDictionary = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _activeEquipItemDictionary = new Dictionary<int, GameObject>();

    int a = 0;
    int b = 0;
    private void Awake()
    {
        foreach(Item_Base itemInfo in _itemList.ItemList)
        {
            var item = Instantiate(_itemTemplate, _itemContent);
            item.SetActive(true);
            item.GetComponent<Image>().sprite = itemInfo.ItemSO.Image;
            _itemDictionary.Add(itemInfo, item);
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                if(itemInfo.ItemSO.ItemType == ItemType.Passive)
                {
                    if (a >= _passiveItemImages.Length) return;
                    for(int i = 0; i < _passiveItemImages.Length; i++)
                    {
                        if(_passiveEquipItemDictionary.ContainsKey(i) == false)
                        {
                            a = i;
                            break;
                        }
                    }
                    _passiveEquipItemDictionary.Add(a, item);
                    _passiveItemImages[a].sprite = itemInfo.ItemSO.Image;
                    _passiveItemImages[a].gameObject.SetActive(true);
                    a = _passiveEquipItemDictionary.Count;
                }
                else
                {
                    if (b >= _activeItemImages.Length) return;
                    for (int i = 0; i < _activeItemImages.Length; i++)
                    {
                        if (_activeEquipItemDictionary.ContainsKey(i) == false)
                        {
                            b = i;
                            break;
                        }
                    }
                    _activeEquipItemDictionary.Add(b, item);
                    _activeItemImages[b].sprite = itemInfo.ItemSO.Image;
                    _activeItemImages[b].gameObject.SetActive(true);
                    b = _activeEquipItemDictionary.Count;
                }

                item.SetActive(false);
            });
        }
    }

    public void OnPassiveUnequip(int idx)
    {
        if (_passiveEquipItemDictionary.ContainsKey(idx) == false) return;
        _passiveEquipItemDictionary[idx].SetActive(true);
        _passiveEquipItemDictionary.Remove(idx);
        _passiveItemImages[idx].gameObject.SetActive(false);
        a = idx;
    }

    public void OnActiveUnequip(int idx)
    {
        if (_activeEquipItemDictionary.ContainsKey(idx) == false) return;
        _activeEquipItemDictionary[idx].SetActive(true);
        _activeEquipItemDictionary.Remove(idx);
        _activeItemImages[idx].gameObject.SetActive(false);
        b = idx;
    }
}
