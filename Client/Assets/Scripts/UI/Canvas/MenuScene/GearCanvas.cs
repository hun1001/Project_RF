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
    [SerializeField]
    private Image[] _passiveLockImages;

    [Space(20f)]
    [SerializeField]
    private Image[] _activeItemImages;
    [SerializeField]
    private Toggle[] _activeItemToggles;
    [SerializeField]
    private Image[] _activeLockImages;

    [Space(20f)]
    [SerializeField]
    private Image _shellImage;
    [SerializeField]
    private Toggle _shellToggle;

    private Dictionary<int, GameObject> _passiveEquipItemDictionary = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _activeEquipItemDictionary = new Dictionary<int, GameObject>();
    private int _passiveSlotIdx = 0;
    private int _activeSlotIdx = 0;

    [Header("Animation")]
    [SerializeField]
    private RectTransform _topPanel;
    [SerializeField]
    private RectTransform _leftPanel;
    [SerializeField]
    private RectTransform _rightPanel;

    private void Awake()
    {
        foreach (Item_Base itemInfo in _itemList.ItemList)
        {
            var item = Instantiate(_itemTemplate, _itemContent);
            item.SetActive(true);
            item.GetComponent<Image>().sprite = itemInfo.ItemSO.Image;
            _itemDictionary.Add(itemInfo, item);
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (itemInfo.ItemSO.ItemType == ItemType.Passive)
                {
                    if(_passiveEquipItemDictionary.ContainsKey(_passiveSlotIdx) == false)
                    {
                        _passiveEquipItemDictionary.Add(_passiveSlotIdx, item);
                        _passiveItemImages[_passiveSlotIdx].gameObject.SetActive(true);
                    }
                    else
                    {
                        _passiveEquipItemDictionary[_passiveSlotIdx].SetActive(true);
                        _passiveEquipItemDictionary[_passiveSlotIdx] = item;
                    }
                    _passiveItemImages[_passiveSlotIdx].sprite = itemInfo.ItemSO.Image;
                }

                else if (itemInfo.ItemSO.ItemType == ItemType.Active)
                {
                    if (_activeEquipItemDictionary.ContainsKey(_activeSlotIdx) == false)
                    {
                        _activeEquipItemDictionary.Add(_activeSlotIdx, item);
                        _activeItemImages[_activeSlotIdx].gameObject.SetActive(true);
                    }
                    else
                    {
                        _activeEquipItemDictionary[_activeSlotIdx].SetActive(true);
                        _activeEquipItemDictionary[_activeSlotIdx] = item;
                    }
                    _activeItemImages[_activeSlotIdx].sprite = itemInfo.ItemSO.Image;
                }

                item.SetActive(false);
                CloseInvetory();
            });
        }
    }

    private void Start()
    {
        _startSequence = DOTween.Sequence()
        .SetAutoKill(false)
        .Prepend(_topPanel.DOAnchorPosY(_topPanel.sizeDelta.y, 0f))
        .Join(_leftPanel.DOAnchorPosX(-_leftPanel.sizeDelta.x, 0f))
        .Join(_rightPanel.DOAnchorPosX(_rightPanel.sizeDelta.x, 0f))
        .Append(_topPanel.DOAnchorPosY(0f, 0.7f))
        .Insert(0.3f, _leftPanel.DOAnchorPosX(0f, 0.5f))
        .Join(_rightPanel.DOAnchorPosX(0f, 0.5f));
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
        _passiveSlotIdx = idx;
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
        _activeSlotIdx = idx;
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
