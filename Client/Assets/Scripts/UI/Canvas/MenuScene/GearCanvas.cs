using Addressable;
using DG.Tweening;
using Item;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum GearType
{
    PassiveItem,
    ActiveItem,
    Shell,
}

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
    private GameObject[] _passiveLock;

    [Space(20f)]
    [SerializeField]
    private Image[] _activeItemImages;
    [SerializeField]
    private Toggle[] _activeItemToggles;
    [SerializeField]
    private GameObject[] _activeLock;

    [Space(20f)]
    [SerializeField]
    private Image[] _shellImages;
    [SerializeField]
    private Toggle[] _shellToggles;

    private GearType _activateGearType;
    private string _currentTankID;
    private Tank _curentTank;
    private ItemEquipmentData _passiveItemEquipmentDataDict;
    private ItemEquipmentData _activeItemEquipmentDataDict;
    private ShellEquipmentData _shellEquipmentDataDict;
    private Dictionary<int, GameObject> _passiveItemEquipSlotDict = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _activeItemEquipSlotDict = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> _shellEquipSlotDict = new Dictionary<int, GameObject>();
    private int _passiveSlotIdx = 0;
    private int _activeSlotIdx = 0;
    private int _shellSlotIdx = 0;
    private uint _passiveItemSlotSize;
    private uint _activeItemSlotSize;

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
        foreach (var item in _itemInventoryDictionary)
        {
            if (item.Key.ItemSO.ItemType == ItemType.Passive)
            {
                if (_passiveItemEquipmentDataDict._itemEquipmentList.Contains(item.Key.ID)) continue;
                item.Value.SetActive(true);
            }
            else if (item.Key.ItemSO.ItemType != ItemType.Passive)
            {
                item.Value.SetActive(false);
            }
        }

        foreach (var shell in _shellInventoryDictionary)
        {
            shell.Value.SetActive(false);
        }

        _inventoryTransform.DOAnchorPosY(0f, 0.7f);
        _passiveItemToggles[idx].isOn = true;
        _passiveSlotIdx = idx;
        _activateGearType = GearType.PassiveItem;
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
        _activateGearType = GearType.ActiveItem;
    }

    public void OnShellInventory(int idx)
    {

        foreach (var item in _itemInventoryDictionary)
        {
            item.Value.SetActive(false);
        }

        foreach (var shell in _shellInventoryDictionary)
        {
            if (_shellEquipmentDataDict._shellEquipmentList.Contains(shell.Key.ID)) continue;
            shell.Value.SetActive(true);
        }

        _inventoryTransform.DOAnchorPosY(0f, 0.7f);
        _shellToggles[idx].isOn = true;
        _shellSlotIdx = idx;
        _activateGearType = GearType.Shell;
    }

    public void OnUnmountGear()
    {
        switch (_activateGearType)
        {
            case GearType.PassiveItem:
                {
                    if (_passiveItemEquipSlotDict.ContainsKey(_passiveSlotIdx))
                    {
                        _passiveItemEquipSlotDict[_passiveSlotIdx].SetActive(true);
                        _passiveItemEquipSlotDict.Remove(_passiveSlotIdx);
                        _passiveItemImages[_passiveSlotIdx].sprite = null;
                        _passiveItemImages[_passiveSlotIdx].gameObject.SetActive(false);
                        ItemSaveManager.ItemEquip(ItemType.Passive, _passiveSlotIdx, "");
                    }
                }
                break;
            case GearType.ActiveItem:
                {
                    if (_activeItemEquipSlotDict.ContainsKey(_activeSlotIdx))
                    {
                        _activeItemEquipSlotDict[_activeSlotIdx].SetActive(true);
                        _activeItemEquipSlotDict.Remove(_activeSlotIdx);
                        _activeItemImages[_activeSlotIdx].sprite = null;
                        _activeItemImages[_activeSlotIdx].gameObject.SetActive(false);
                        ItemSaveManager.ItemEquip(ItemType.Active, _activeSlotIdx, "");
                    }
                }
                break;
            case GearType.Shell:
                {
                    if (_shellEquipSlotDict.ContainsKey(_shellSlotIdx))
                    {
                        _shellEquipSlotDict[_shellSlotIdx].SetActive(true);
                        _shellEquipSlotDict.Remove(_shellSlotIdx);
                        _shellImages[_shellSlotIdx].sprite = null;
                        _shellImages[_shellSlotIdx].gameObject.SetActive(false);
                        ShellSaveManager.ShellEquip(_currentTankID, _shellSlotIdx, "");
                    }
                }
                break;
        }
    }

    public void CloseInvetory(bool isInstant)
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
        if (isInstant)
        {
            _inventoryTransform.DOAnchorPosY(-_inventoryTransform.sizeDelta.y, 0f);
        }
        else
        {
            _inventoryTransform.DOAnchorPosY(-_inventoryTransform.sizeDelta.y, 0.7f);
        }
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
        CloseInvetory(true);
    }

    private void AddItems()
    {
        ResetItem();

        PassiveAddItem();
        ActiveAddItem();
        LockItem();

        AddShell();
    }

    private void ResetItem()
    {
        _currentTankID = PlayerDataManager.Instance.GetPlayerTankID();
        _curentTank = FindObjectOfType<TankModelManager>().TankModel;
        _shellEquipmentDataDict = ShellSaveManager.GetShellEquipment(_currentTankID);
        _passiveItemSlotSize = _curentTank.TankSO.PassiveItemInventorySize;
        _activeItemSlotSize = _curentTank.TankSO.ActiveItemInventorySize;

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
                if (_passiveItemSlotSize >= idx + 1)
                {
                    _passiveItemEquipSlotDict.Add(idx, item);
                    item.SetActive(false);

                    _passiveItemImages[idx].gameObject.SetActive(true);
                    _passiveItemImages[idx].sprite = itemInfo.ItemSO.Image;
                }
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
                CloseInvetory(false);
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
                if (_passiveItemSlotSize >= idx + 1)
                {
                    _activeItemEquipSlotDict.Add(idx, item);
                    item.SetActive(false);

                    _activeItemImages[idx].gameObject.SetActive(true);
                    _activeItemImages[idx].sprite = itemInfo.ItemSO.Image;
                }
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
                CloseInvetory(false);
            });
        }
    }

    private void LockItem()
    {
        for (int i = _passiveLock.Length - 1; i >= _passiveItemSlotSize; i--)
        {
            _passiveLock[i].SetActive(true);
            if (_passiveItemEquipSlotDict.ContainsKey(i))
            {
                _passiveItemEquipSlotDict.Remove(i);
                _passiveItemImages[i].sprite = null;
                _passiveItemImages[i].gameObject.SetActive(false);
            }
        }

        for (int i = _activeLock.Length - 1; i >= _activeItemSlotSize; i--)
        {
            _activeLock[i].SetActive(true);
            if (_activeItemEquipSlotDict.ContainsKey(i))
            {
                _activeItemEquipSlotDict.Remove(i);
                _activeItemImages[i].sprite = null;
                _activeItemImages[i].gameObject.SetActive(false);
            }
        }
    }

    private void AddShell()
    {
        Turret turret = AddressablesManager.Instance.GetResource<GameObject>(_currentTankID).GetComponent<Turret>();
        foreach (Shell shellInfo in turret.TurretSO.Shells)
        {
            var shell = Instantiate(_itemTemplate, _itemContent);
            shell.SetActive(true);
            shell.GetComponent<Image>().sprite = shellInfo.ShellSprite;
            _shellInventoryDictionary.Add(shellInfo, shell);

            if (_shellEquipmentDataDict._shellEquipmentList.Contains(shellInfo.ID))
            {
                int idx = _shellEquipmentDataDict._shellEquipmentList.IndexOf(shellInfo.ID);
                _shellEquipSlotDict.Add(idx, shell);
                shell.SetActive(false);

                _shellImages[idx].gameObject.SetActive(true);
                _shellImages[idx].sprite = shellInfo.ShellSprite;
            }

            shell.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_shellEquipmentDataDict._shellEquipmentList[_shellSlotIdx] == "")
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

                if (_shellEquipmentDataDict._shellEquipmentList.Contains(""))
                {
                    _shellToggles[_shellSlotIdx].isOn = false;
                    _shellSlotIdx = _shellEquipmentDataDict._shellEquipmentList.IndexOf("");
                    _shellToggles[_shellSlotIdx].isOn = true;
                }
                else
                {
                    CloseInvetory(false);
                }
            });
        }
    }
}
