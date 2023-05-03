using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Item;
using Util;
using System.Collections.Generic;

public enum ShopToggle
{
    PremiumTank,
    Item,
    RandomGacha,
    PaidGoods,
}

public class ShopCanvas : BaseCanvas
{
    [Header("Scroll Rect")]
    [SerializeField]
    private ScrollRect _scrollRect;

    [Header("Toggle")]
    [SerializeField]
    private ToggleGroup _toggleGroup;
    [SerializeField]
    private Toggle _premiumTankToggle;
    [SerializeField]
    private Toggle _itemToggle;
    [SerializeField]
    private Toggle _randomGachaToggle;
    [SerializeField]
    private Toggle _paidGoodsToggle;

    [Header("Button")]
    [SerializeField]
    private Button _premiumTankButton;
    [SerializeField]
    private Button _itemButton;
    [SerializeField]
    private Button _randomGachaButton;
    [SerializeField]
    private Button _paidGoodsButton;

    [Header("Animation")]
    [SerializeField]
    private RectTransform _content;
    private Image[] _toggleImages;

    [Header("Product")]
    [SerializeField]
    private GameObject _premiumTankTemplate;
    [SerializeField]
    private RectTransform _premiumTankTransform;

    [Space(10f)]
    [SerializeField]
    private ItemListSO _itemListSO;
    [SerializeField]
    private GameObject _itemTemplate;
    [SerializeField]
    private RectTransform _itemTransform;

    private Item_Base _selectItem;
    private Dictionary<Item_Base, GameObject> _showingItemList = new Dictionary<Item_Base, GameObject>();
    private WeightedRandomPicker<Item_Base> _itemPicker = new WeightedRandomPicker<Item_Base>();

    [Space(10f)]
    [SerializeField]
    private GameObject _paidGoodsTemplate;
    [SerializeField]
    private RectTransform _paidGoodsTransform;

    [Space(10f)]
    [SerializeField]
    private GameObject _productInformation;
    [SerializeField]
    private GameObject _productBuy;

    private void Awake()
    {
        ItemInventoryData passiveInventoryData = ItemSaveManager.GetItemInventory(ItemType.Passive);
        ItemInventoryData activeInventoryData = ItemSaveManager.GetItemInventory(ItemType.Active);

        _itemPicker.Clear();
        foreach(Item_Base item in _itemListSO.ItemList)
        {
            if(passiveInventoryData._itemInventoryList.Contains(item.ID) == false && activeInventoryData._itemInventoryList.Contains(item.ID) == false)
            {
                _itemPicker.Add(item, 1.0);
            }
        }

        _scrollRect.onValueChanged.AddListener(OnScroll);
        _premiumTankButton.onClick.AddListener(delegate { OnToggle(ShopToggle.PremiumTank); });
        _itemButton.onClick.AddListener(delegate { OnToggle(ShopToggle.Item); });
        _randomGachaButton.onClick.AddListener(delegate { OnToggle(ShopToggle.RandomGacha); });
        _paidGoodsButton.onClick.AddListener(delegate { OnToggle(ShopToggle.PaidGoods); });

        _toggleImages = _toggleGroup.GetComponentsInChildren<Image>();

        for(int i = 0; i < 6; i++)
        {
            var product = Instantiate(_premiumTankTemplate, _premiumTankTransform);
            product.SetActive(true);
        }

        _showingItemList.Clear();
        if(_itemPicker.GetLength() >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                var product = Instantiate(_itemTemplate, _itemTransform);

                Item_Base item;
                while (true)
                {
                    item = _itemPicker.GetRandomPick();
                    if (_showingItemList.ContainsKey(item) == false) break;
                }
                _showingItemList.Add(item, product);

                product.transform.GetChild(0).GetComponent<Image>().sprite = item.ItemSO.Image;
                product.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.ItemSO.Name;

                product.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    _selectItem = item;
                    _productBuy.SetActive(true);
                });

                product.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < _itemPicker.GetLength(); i++)
            {
                var product = Instantiate(_itemTemplate, _itemTransform);

                Item_Base item;
                while (true)
                {
                    item = _itemPicker.GetRandomPick();
                    if (_showingItemList.ContainsKey(item) == false) break;
                }
                _showingItemList.Add(item, product);

                product.transform.GetChild(0).GetComponent<Image>().sprite = item.ItemSO.Image;
                product.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = item.ItemSO.Name;

                product.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    _selectItem = item;
                    _productBuy.SetActive(true);
                });

                product.SetActive(true);
            }
        }

        for (int i = 0; i < 6; i++)
        {
            var product = Instantiate(_paidGoodsTemplate, _paidGoodsTransform);
            product.SetActive(true);
        }
    }

    private void Start()
    {
        _scrollRect.normalizedPosition = Vector2.zero;
    }

    public override void OnOpenEvents()
    {
        _startSequence = DOTween.Sequence()
        .PrependCallback(() =>
        {
            _scrollRect.onValueChanged.RemoveAllListeners();
            _content.anchoredPosition += Vector2.down * 300f;

            for (int i = 3; i < _toggleImages.Length; i += 3)
            {
                _toggleImages[i].DOFade(0f, 0f);
            }
        })
        .Insert(0f, _content.DOAnchorPosY(0f, 1f))
        .Insert(0.25f, _toggleImages[3].DOFade(1f, 1f))
        .Insert(0.5f, _toggleImages[6].DOFade(1f, 1f))
        .Insert(0.75f, _toggleImages[9].DOFade(1f, 1f))
        .AppendCallback(() => _scrollRect.onValueChanged.AddListener(OnScroll));
    }

    // 0 ~ 1 값이 전달된다.
    public void OnScroll(Vector2 vector2)
    {
        float x = (float)Math.Round(vector2.x, 1);
        
        switch (x)
        {
            case 0f:
                {
                    _premiumTankToggle.isOn = true;
                }
                break;
            case 0.3f:
                {
                    _itemToggle.isOn = true;
                }
                break;
            case 0.7f:
                {
                    _randomGachaToggle.isOn = true;
                }
                break;
            case 1f:
                {
                    _paidGoodsToggle.isOn = true;
                }
                break;
        }
    }

    public void OnToggle(ShopToggle value)
    {
        switch (value)
        {
            case ShopToggle.PremiumTank:
                {
                    _scrollRect.normalizedPosition = Vector2.zero;
                }
                break;
            case ShopToggle.Item:
                {
                    _scrollRect.normalizedPosition = new Vector2(0.42f, 0f);
                }
                break;
            case ShopToggle.RandomGacha:
                {
                    _scrollRect.normalizedPosition = new Vector2(0.71f, 0f);
                }
                break;
            case ShopToggle.PaidGoods:
                {
                    _scrollRect.normalizedPosition = Vector2.right;
                }
                break;
            default: break;
        }
    }

    public override void OnHomeButton()
    {
        base.OnHomeButton();
        _scrollRect.normalizedPosition = Vector2.zero;
        _productInformation.SetActive(false);
        _productBuy.SetActive(false);
        _selectItem = null;
    }

    public override void OnBackButton()
    {
        base.OnBackButton();
        _scrollRect.normalizedPosition = Vector2.zero;
        _productInformation.SetActive(false);
        _productBuy.SetActive(false);
        _selectItem = null;
    }

    public void OnProductBuy()
    {
        ItemSaveManager.BuyItem(_selectItem.ItemSO.ItemType, _selectItem.ID);
        var product = _showingItemList[_selectItem];
        _showingItemList.Remove(_selectItem);
        Destroy(product);
        OnDontProductBuy();
    }

    public void OnDontProductBuy()
    {
        _productBuy.SetActive(false);
        _selectItem = null;
    }
}
