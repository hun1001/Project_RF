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
    private Dictionary<Item_Base, GameObject> _itemInventoryDictionary = new Dictionary<Item_Base, GameObject>();
    private Dictionary<Shell, GameObject> _shellInventoryDictionary = new Dictionary<Shell, GameObject>();

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

    private string _currentTankID;
    private ItemEquipmentData _passiveItemEquipmentDataDict;
    private ItemEquipmentData _activeItemEquipmentDataDict;
    private ShellEquipmentData _shellEquipmentDataDict;
    private Dictionary<int, GameObject> _passiveItemEquipSlotDict = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _activeItemEquipSlotDict = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _shellEquipSlotDict = new Dictionary<int, GameObject>();
    private int _passiveSlotIdx = 0;
    private int _activeSlotIdx = 0;
    private int _shellSlotIdx = 0;

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

    public void OnPassiveInventory(int idx)
    {
        foreach(var item in _itemInventoryDictionary)
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

        foreach (var shell in  _shellInventoryDictionary)
        {
            shell.Value.SetActive(false);
        }

        _inventoryTransform.DOAnchorPosY(0f, 0.7f);
        _passiveItemToggles[idx].isOn = true;
        _passiveSlotIdx = idx;
    }

    public void OnActiveInventory(int idx)
    {
        foreach (var item in _itemInventoryDictionary)
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

        foreach (var shell in _shellInventoryDictionary)
        {
            shell.Value.SetActive(false);
        }

        _inventoryTransform.DOAnchorPosY(0f, 0.7f);
        _activeItemToggles[idx].isOn = true;
        _activeSlotIdx = idx;
    }

    public void OnShellInventory(int idx)
    {

        foreach (var item in _itemInventoryDictionary)
        {
            item.Value.SetActive(false);
        }

        foreach (var shell in _shellInventoryDictionary)
        {
            if (_shellEquipmentDataDict._shellEquipmentData.Contains(shell.Key.ID)) continue;
            shell.Value.SetActive(true);
        }

        _inventoryTransform.DOAnchorPosY(0f, 0.7f);
        _shellToggles[idx].isOn = true;
        _shellSlotIdx = idx;
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

        _startSequence = DOTween.Sequence()
        .Prepend(_topPanel.DOAnchorPosY(_topPanel.sizeDelta.y, 0f))
        .Join(_leftPanel.DOAnchorPosX(-_leftPanel.sizeDelta.x, 0f))
        .Join(_rightPanel.DOAnchorPosX(_rightPanel.sizeDelta.x, 0f))
        .Append(_topPanel.DOAnchorPosY(0f, 0.7f))
        .Insert(0.3f, _leftPanel.DOAnchorPosX(0f, 0.5f))
        .Join(_rightPanel.DOAnchorPosX(0f, 0.5f));

        AddItems();
    }

    private void AddItems()
    {
        ResetItem();

        PassiveAddItem();
        ActiveAddItem();
        AddShell();
    }

    private void ResetItem()
    {
        // √ ±‚»≠
        foreach (var item in _itemInventoryDictionary)
        {
            Destroy(item.Value);
        }
        _itemInventoryDictionary.Clear();
        foreach (var shell in _shellInventoryDictionary)
        {
            Destroy(shell.Value);
        }
        _shellInventoryDictionary.Clear();

        _passiveItemEquipSlotDict.Clear();
        _activeItemEquipSlotDict.Clear();
        _shellEquipSlotDict.Clear();

        foreach (var image in _shellImages)
        {
            image.sprite = null;
            image.gameObject.SetActive(false);
        }
    }

    private void PassiveAddItem()
    {
        ItemInventoryData passiveInventoryData = ItemSaveManager.GetItemInventory(ItemType.Passive);

        foreach (var itemName in passiveInventoryData._itemInventoryList)
        {
            Item_Base itemInfo = AddressablesManager.Instance.GetResource<GameObject>(itemName).GetComponent<Item_Base>();

            var item = Instantiate(_itemTemplate, _itemContent);
            item.SetActive(true);
            item.GetComponent<Image>().sprite = itemInfo.ItemSO.Image;
            _itemInventoryDictionary.Add(itemInfo, item);

            if (_passiveItemEquipmentDataDict._itemEquipmentList.Contains(itemInfo.ID))
            {
                int idx = _passiveItemEquipmentDataDict._itemEquipmentList.IndexOf(itemInfo.ID);
                _passiveItemEquipSlotDict.Add(idx, item);
                item.SetActive(false);

                _passiveItemImages[idx].gameObject.SetActive(true);
                _passiveItemImages[idx].sprite = itemInfo.ItemSO.Image;
            }

            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_passiveItemEquipmentDataDict._itemEquipmentList[_passiveSlotIdx] == "")
                {
                    ItemSaveManager.ItemEquip(ItemType.Passive, _passiveSlotIdx, itemInfo.ID);
                    _passiveItemEquipSlotDict.Add(_passiveSlotIdx, item);
                    _passiveItemImages[_passiveSlotIdx].gameObject.SetActive(true);
                }
                else
                {
                    _passiveItemEquipSlotDict[_passiveSlotIdx].SetActive(true);
                    ItemSaveManager.ItemEquip(ItemType.Passive, _passiveSlotIdx, itemInfo.ID);
                    _passiveItemEquipSlotDict[_passiveSlotIdx] = item;
                }
                _passiveItemImages[_passiveSlotIdx].sprite = itemInfo.ItemSO.Image;

                item.SetActive(false);
                CloseInvetory();
            });
        }
    }

    private void ActiveAddItem()
    {
        ItemInventoryData activeInventoryData = ItemSaveManager.GetItemInventory(ItemType.Active);

        foreach (var itemName in activeInventoryData._itemInventoryList)
        {
            Item_Base itemInfo = AddressablesManager.Instance.GetResource<GameObject>(itemName).GetComponent<Item_Base>();

            var item = Instantiate(_itemTemplate, _itemContent);
            item.SetActive(true);
            item.GetComponent<Image>().sprite = itemInfo.ItemSO.Image;
            _itemInventoryDictionary.Add(itemInfo, item);

            if (_activeItemEquipmentDataDict._itemEquipmentList.Contains(itemInfo.ID))
            {
                int idx = _activeItemEquipmentDataDict._itemEquipmentList.IndexOf(itemInfo.ID);
                _activeItemEquipSlotDict.Add(idx, item);
                item.SetActive(false);

                _activeItemImages[idx].gameObject.SetActive(true);
                _activeItemImages[idx].sprite = itemInfo.ItemSO.Image;
            }

            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_activeItemEquipmentDataDict._itemEquipmentList[_activeSlotIdx] == "")
                {
                    ItemSaveManager.ItemEquip(ItemType.Active, _activeSlotIdx, itemInfo.ID);
                    _activeItemEquipSlotDict.Add(_activeSlotIdx, item);
                    _activeItemImages[_activeSlotIdx].gameObject.SetActive(true);
                }
                else
                {
                    _activeItemEquipSlotDict[_activeSlotIdx].SetActive(true);
                    ItemSaveManager.ItemEquip(ItemType.Active, _activeSlotIdx, itemInfo.ID);
                    _activeItemEquipSlotDict[_activeSlotIdx] = item;
                }
                _activeItemImages[_activeSlotIdx].sprite = itemInfo.ItemSO.Image;

                item.SetActive(false);
                CloseInvetory();
            });
        }
    }

    private void AddShell()
    {
        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();
        _shellEquipmentDataDict = ShellSaveManager.GetShellEquipment(_currentTankID);

        Turret _turret = AddressablesManager.Instance.GetResource<GameObject>(_currentTankID).GetComponent<Turret>();
        foreach (Shell shellInfo in _turret.TurretSO.Shells)
        {
            var shell = Instantiate(_itemTemplate, _itemContent);
            shell.SetActive(true);
            shell.GetComponent<Image>().sprite = shellInfo.ShellSprite;
            _shellInventoryDictionary.Add(shellInfo, shell);

            if (_shellEquipmentDataDict._shellEquipmentData.Contains(shellInfo.ID))
            {
                int idx = _shellEquipmentDataDict._shellEquipmentData.IndexOf(shellInfo.ID);
                _shellEquipSlotDict.Add(idx, shell);
                shell.SetActive(false);

                _shellImages[idx].gameObject.SetActive(true);
                _shellImages[idx].sprite = shellInfo.ShellSprite;
            }

            shell.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_shellEquipmentDataDict._shellEquipmentData[_shellSlotIdx] == "")
                {
                    ShellSaveManager.ShellEquip(_currentTankID, _shellSlotIdx, shellInfo.ID);
                    _shellEquipSlotDict.Add(_shellSlotIdx, shell);
                    _shellImages[_shellSlotIdx].gameObject.SetActive(true);
                }
                else
                {
                    _shellEquipSlotDict[_shellSlotIdx].SetActive(true);
                    ShellSaveManager.ShellEquip(_currentTankID, _shellSlotIdx, shellInfo.ID);
                    _shellEquipSlotDict[_shellSlotIdx] = shell;
                }
                _shellImages[_shellSlotIdx].sprite = shellInfo.ShellSprite;

                shell.SetActive(false);
                CloseInvetory();
            });
        }
    }
}
