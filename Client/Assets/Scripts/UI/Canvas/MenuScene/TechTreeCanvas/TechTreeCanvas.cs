using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class TechTreeCanvas : BaseCanvas
{
    [Header("Goods")]
    [SerializeField]
    private GoodsTexts _goodsTexts = null;

    [Header("Tier")]
    [SerializeField]
    private TechTreeTierLine _tankTierLine = null;

    [Header("TechTree")]
    [SerializeField]
    private TechTree _techTree = null;

    [SerializeField]
    private TechTreeCountryToggles _countryToggleGroup = null;

    [SerializeField]
    private RectTransform _tankNodeContentTransform = null;

    [SerializeField]
    private RectTransform _techTreeScrollView = null;
    private ScrollRect _scrollRect = null;

    [SerializeField]
    private TextController _countryTextController = null;

    [SerializeField]
    private GameObject _tankInformation = null;
    [SerializeField]
    private GameObject _tankInformationPanel = null;

    private GameObject _tankNodeRowTemplate = null;
    private GameObject _tankNodeTemplate = null;
    private GameObject _tankNodeConnectHorizontalLineTemplate = null;
    private GameObject _tankNodeNullTemplate = null;
    private GameObject _tankNodeConnectHorizontalNullLineTemplate = null;

    private void Awake()
    {
        _tankInformation.SetActive(false);
        _tankInformation.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            _tankInformation.SetActive(false);
            PlayButtonSound();
        });
        _tankInformationPanel.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() =>
        {
            _tankInformation.SetActive(false);
            PlayButtonSound();
        });

        _goodsTexts.SetGoodsTexts(GoodsManager.FreeGoods, GoodsManager.PaidGoods);


        _tankNodeRowTemplate = _tankNodeContentTransform.GetChild(0).gameObject;
        _tankNodeTemplate = _tankNodeRowTemplate.transform.GetChild(0).gameObject;
        _tankNodeConnectHorizontalLineTemplate = _tankNodeRowTemplate.transform.GetChild(1).gameObject;
        _tankNodeNullTemplate = _tankNodeRowTemplate.transform.GetChild(2).gameObject;
        _tankNodeConnectHorizontalNullLineTemplate = _tankNodeRowTemplate.transform.GetChild(3).gameObject;

        _tankNodeRowTemplate.SetActive(false);
        _tankNodeTemplate.SetActive(false);
        _tankNodeConnectHorizontalLineTemplate.SetActive(false);
        _tankNodeNullTemplate.SetActive(false);
        _tankNodeConnectHorizontalNullLineTemplate.SetActive(false);

        _techTreeScrollView.TryGetComponent(out _scrollRect);

        for (int i = 0; i < _techTree.TechTreeSO.Length; ++i)
        {
            int index = i;

            _countryToggleGroup.CreateCountryToggles(_techTree.TechTreeSO[index].FlagSprite, () => SetTechTree(index));
        }

        _countryToggleGroup.ChangeFirstToggleValue(true);
    }

    private void SetTechTree(int index)
    {
        PlayButtonSound();
        _countryTextController.SetText(_techTree.TechTreeSO[index].CountryType.ToString());

        ResetTankNode();
        _tankTierLine.ResetTierLine();

        _tankTierLine.SetTierLine(_techTree.TechTreeSO[index].GetMaximumLength());

        for (int j = 0; j < _techTree.TechTreeSO[index].Length; ++j)
        {
            int jIndex = j;
            var rowTransform = Instantiate(_tankNodeRowTemplate, _tankNodeContentTransform).transform;

            for (int l = 0; l < _techTree.TechTreeSO[index].GetTankArrayLength(jIndex); ++l)
            {
                int lIndex = l;

                GameObject node;

                if (_techTree.TechTreeSO[index][jIndex, lIndex] == null)
                {
                    node = Instantiate(_tankNodeNullTemplate, rowTransform);
                    if (lIndex != _techTree.TechTreeSO[index].GetTankArrayLength(jIndex) - 1)
                    {
                        if (_techTree.TechTreeSO[index][jIndex, lIndex + 1] == null)
                        {
                            var tankNodeConnectLine = Instantiate(_tankNodeConnectHorizontalNullLineTemplate, rowTransform);
                            tankNodeConnectLine.SetActive(true);
                        }
                        else
                        {
                            var tankNodeConnectLine = Instantiate(_tankNodeConnectHorizontalLineTemplate, rowTransform);
                            tankNodeConnectLine.SetActive(true);
                        }
                    }
                }
                else
                {
                    node = Instantiate(_tankNodeTemplate, rowTransform);

                    var tNC = node.GetComponent<TankNode>();

                    bool isLock = !TechTreeDataManager.GetTechTreeProgress(_techTree.TechTreeSO[index].CountryType)._tankProgressList.Contains(_techTree.TechTreeSO[index][jIndex, lIndex].ID);

                    tNC.SetTankNode(_techTree.GetTankTypeSprite(_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.TankType), _techTree.TankTierNumber[lIndex], _techTree.TechTreeSO[index][jIndex, lIndex].ID, isLock, () =>
                    {
                        PlayButtonSound();

                        _tankInformation.SetActive(true);
                        var topUI = _tankInformationPanel.transform.GetChild(0);
                        topUI.GetChild(0).GetComponent<Image>().sprite = _techTree.GetTankTypeSprite(_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.TankType);
                        topUI.GetChild(1).GetComponent<Text>().text = _techTree.TankTierNumber[lIndex];
                        topUI.GetChild(2).GetComponent<Text>().text = _techTree.TechTreeSO[index][jIndex, lIndex].ID;

                        // 탱크 이미지 없으니까 일단  null
                        _tankInformationPanel.transform.GetChild(1).GetComponent<Image>().sprite = null;

                        _tankInformationPanel.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                        _tankInformationPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            if (TechTreeDataManager.GetTechTreeProgress(_techTree.TechTreeSO[index].CountryType)._tankProgressList.Contains(_techTree.TechTreeSO[index][jIndex, lIndex].ID))
                            {
                                FindObjectOfType<TankModelManager>().ChangeTankModel(_techTree.TechTreeSO[index][jIndex, lIndex]);
                                _tankInformation.SetActive(false);
                            }
                            PlayButtonSound();
                        });

                        _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
                        _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
                        {
                            TechTreeDataManager.AddTank(_techTree.TechTreeSO[index].CountryType, _techTree.TechTreeSO[index][jIndex, lIndex].ID);
                            tNC.IsTankLocked = false;
                            PlayButtonSound();
                        });
                    });

                    node.GetComponent<Image>().enabled = true;

                    if (lIndex != _techTree.TechTreeSO[index].GetTankArrayLength(jIndex) - 1)
                    {
                        var tankNodeConnectLine = Instantiate(_tankNodeConnectHorizontalLineTemplate, rowTransform);
                        tankNodeConnectLine.SetActive(true);
                    }
                }

                node.SetActive(true);
            }

            rowTransform.GetComponent<HorizontalLayoutGroup>().enabled = true;
            rowTransform.GetComponent<ContentSizeFitter>().enabled = true;
            rowTransform.gameObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)rowTransform.transform);
        }
    }

    private void ResetTankNode()
    {
        for (int k = 2; k < _tankNodeContentTransform.transform.childCount; ++k)
        {
            Destroy(_tankNodeContentTransform.transform.GetChild(k).gameObject);
        }
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();

        //int idx = 1;
        _startSequence = DOTween.Sequence()
        .PrependCallback(() =>
        {
            _techTreeScrollView.anchoredPosition = Vector2.right * 1000f;

            //idx = 1;
            //foreach (RectTransform rect in _toggleList)
            //{
            //    rect.anchoredPosition += Vector2.down * 100f * idx++;
            //}
        })
        .Append(_techTreeScrollView.DOAnchorPosX(0f, 1f))
        //.InsertCallback(0.5f, () =>
        //{
        //idx = 1;
        //foreach (RectTransform rect in _toggleList)
        //{
        //    rect.DOAnchorPosY(-25f, 0.2f * idx++);
        //}
        //})
        .AppendCallback(() => _scrollRect.normalizedPosition = Vector2.zero);

        _tankInformation.SetActive(false);
    }

    public override void OnBackButton()
    {
        _tankInformation.SetActive(false);
        base.OnBackButton();
    }
}
