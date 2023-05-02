using Addressable;
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
    private Image[] _shellImages;
    [SerializeField]
    private Toggle[] _shellToggles;

    private ItemEquipmentData _passiveItemEquipmentDataDict;
    private ItemEquipmentData _activeItemEquipmentDataDict;
    private Dictionary<int, GameObject> _passiveItemDict = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _activeItemDict = new Dictionary<int, GameObject>();
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
        _passiveItemEquipmentDataDict = ItemSaveManager.GetItemEquipment(ItemType.Passive);
        _activeItemEquipmentDataDict = ItemSaveManager.GetItemEquipment(ItemType.Active);

        AddItems();
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
                if (_passiveItemEquipmentDataDict._itemEquipmentList.Contains(item.Key.ID)) continue;
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
                if (_activeItemEquipmentDataDict._itemEquipmentList.Contains(item.Key.ID)) continue;
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

    public void OnShellInventory(int idx)
    {
        _inventoryTransform.DOAnchorPosY(0f, 0.7f);
        _shellToggles[idx].isOn = true;
    }

    public void CloseInvetory()
    {
        foreach (var toggle in _passiveItemToggles)
        {
            toggle.isOn = false;
        }
        foreach (var toggle in _activeItemToggles)
        {
            toggle.isOn = false;
        }
        foreach (var toggle in _shellToggles)
        {
            toggle.isOn = false;
        }

        _inventoryTransform.DOAnchorPosY(-_inventoryTransform.sizeDelta.y, 0.7f);
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();
        AddItems();
    }

    private void AddItems()
    {
        ItemInventoryData passiveInventoryData = ItemSaveManager.GetItemInventory(ItemType.Passive);
        ItemInventoryData activeInventoryData = ItemSaveManager.GetItemInventory(ItemType.Active);

        foreach (var itemName in passiveInventoryData._itemInventoryList)
        {
            Item_Base itemInfo = AddressablesManager.Instance.GetResource<GameObject>(itemName).GetComponent<Item_Base>();
            if (_itemDictionary.ContainsKey(itemInfo))
            {
                continue;
            }

            var item = Instantiate(_itemTemplate, _itemContent);
            item.SetActive(true);
            item.GetComponent<Image>().sprite = itemInfo.ItemSO.Image;
            _itemDictionary.Add(itemInfo, item);

            if (_passiveItemEquipmentDataDict._itemEquipmentList.Contains(itemInfo.ID))
            {
                int idx = _passiveItemEquipmentDataDict._itemEquipmentList.IndexOf(itemInfo.ID);
                _passiveItemDict.Add(idx, item);
                item.SetActive(false);

                _passiveItemImages[idx].gameObject.SetActive(true);
                _passiveItemImages[idx].sprite = itemInfo.ItemSO.Image;
            }

            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_passiveItemEquipmentDataDict._itemEquipmentList[_passiveSlotIdx] == "")
                {
                    ItemSaveManager.ItemEquip(ItemType.Passive, _passiveSlotIdx, itemInfo.ID);
                    _passiveItemDict.Add(_passiveSlotIdx, item);
                    _passiveItemImages[_passiveSlotIdx].gameObject.SetActive(true);
                }
                else
                {
                    _passiveItemDict[_passiveSlotIdx].SetActive(true);
                    ItemSaveManager.ItemEquip(ItemType.Passive, _passiveSlotIdx, itemInfo.ID);
                    _passiveItemDict[_passiveSlotIdx] = item;
                }
                _passiveItemImages[_passiveSlotIdx].sprite = itemInfo.ItemSO.Image;

                item.SetActive(false);
                CloseInvetory();
            });
        }

        foreach (var itemName in activeInventoryData._itemInventoryList)
        {
            Item_Base itemInfo = AddressablesManager.Instance.GetResource<GameObject>(itemName).GetComponent<Item_Base>();
            if (_itemDictionary.ContainsKey(itemInfo))
            {
                continue;
            }

            var item = Instantiate(_itemTemplate, _itemContent);
            item.SetActive(true);
            item.GetComponent<Image>().sprite = itemInfo.ItemSO.Image;
            _itemDictionary.Add(itemInfo, item);

            if (_activeItemEquipmentDataDict._itemEquipmentList.Contains(itemInfo.ID))
            {
                int idx = _activeItemEquipmentDataDict._itemEquipmentList.IndexOf(itemInfo.ID);
                _activeItemDict.Add(idx, item);
                item.SetActive(false);

                _activeItemImages[idx].gameObject.SetActive(true);
                _activeItemImages[idx].sprite = itemInfo.ItemSO.Image;
            }

            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_activeItemEquipmentDataDict._itemEquipmentList[_activeSlotIdx] == "")
                {
                    ItemSaveManager.ItemEquip(ItemType.Active, _activeSlotIdx, itemInfo.ID);
                    _activeItemDict.Add(_activeSlotIdx, item);
                    _activeItemImages[_activeSlotIdx].gameObject.SetActive(true);
                }
                else
                {
                    _activeItemDict[_activeSlotIdx].SetActive(true);
                    ItemSaveManager.ItemEquip(ItemType.Active, _activeSlotIdx, itemInfo.ID);
                    _activeItemDict[_activeSlotIdx] = item;
                }
                _activeItemImages[_activeSlotIdx].sprite = itemInfo.ItemSO.Image;

                item.SetActive(false);
                CloseInvetory();
            });
        }
    }
}
