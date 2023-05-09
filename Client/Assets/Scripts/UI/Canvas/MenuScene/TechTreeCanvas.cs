using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TechTreeCanvas : BaseCanvas
{
    [SerializeField]
    private TechTree _techTree = null;

    [SerializeField]
    private ToggleGroup _countryToggleGroupManager = null;

    [SerializeField]
    private RectTransform _tankNodeContentTransform = null;

    [SerializeField]
    private RectTransform _techTreeScrollView = null;
    private ScrollRect _scrollRect = null;

    [SerializeField]
    private GameObject _tankInformation = null;
    [SerializeField]
    private GameObject _tankInformationPanel = null;

    private GameObject _countryToggleTemplate = null;

    private GameObject _tankNodeRowTemplate = null;
    private GameObject _tankNodeTemplate = null;
    private GameObject _tankNodeConnectHorizontalLineTemplate = null;
    private GameObject _tankNodeNullTemplate = null;
    private GameObject _tankNodeConnectHorizontalNullLineTemplate = null;

    private List<RectTransform> _toggleList = new List<RectTransform>();

    private void Awake()
    {
        _tankInformation.SetActive(false);
        _tankInformation.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            _tankInformation.SetActive(false);
        });

        _countryToggleTemplate = _countryToggleGroupManager.transform.GetChild(0).gameObject;

        _tankNodeRowTemplate = _tankNodeContentTransform.GetChild(0).gameObject;
        _tankNodeTemplate = _tankNodeRowTemplate.transform.GetChild(0).gameObject;
        _tankNodeConnectHorizontalLineTemplate = _tankNodeRowTemplate.transform.GetChild(1).gameObject;
        _tankNodeNullTemplate = _tankNodeRowTemplate.transform.GetChild(2).gameObject;
        _tankNodeConnectHorizontalNullLineTemplate = _tankNodeRowTemplate.transform.GetChild(3).gameObject;

        _countryToggleTemplate.SetActive(false);
        _tankNodeRowTemplate.SetActive(false);
        _tankNodeTemplate.SetActive(false);
        _tankNodeConnectHorizontalLineTemplate.SetActive(false);
        _tankNodeNullTemplate.SetActive(false);
        _tankNodeConnectHorizontalNullLineTemplate.SetActive(false);

        _scrollRect = _techTreeScrollView.GetComponent<ScrollRect>();

        for (int i = 0; i < _techTree.TechTreeSO.Length; ++i)
        {
            int index = i;
            var countryToggle = Instantiate(_countryToggleTemplate, _countryToggleGroupManager.transform).GetComponent<Toggle>();
            countryToggle.transform.GetChild(0).GetComponent<Image>().sprite = _techTree.TechTreeSO[index].FlagSprite;

            countryToggle.onValueChanged.AddListener((isOn) =>
            {
                for (int j = 1; j < _tankNodeContentTransform.childCount; ++j)
                {
                    Destroy(_tankNodeContentTransform.GetChild(j).gameObject);
                }

                if (isOn)
                {
                    for (int r = 0; r < _techTree.TechTreeSO[index].RootTankNodes.Length; ++r)
                    {
                        int rIndex = r;
                        TankNode tankNode = _techTree.TechTreeSO[index].RootTankNodes[r];

                        var tankNodeRowTemplate = Instantiate(_tankNodeRowTemplate, _tankNodeContentTransform);

                        TankTechTreeNode tankNodeUI = null;
                        GameObject tankNodeConnectHorizontalLineUI = null;

                        while (tankNode != null)
                        {
                            tankNodeUI = Instantiate(_tankNodeTemplate, tankNodeRowTemplate.transform).GetComponent<TankTechTreeNode>();
                            tankNodeConnectHorizontalLineUI = Instantiate(_tankNodeConnectHorizontalLineTemplate, tankNodeRowTemplate.transform);

                            Tank tankInfo = tankNode.Tank;

                            tankNodeUI.SetTankNode(_techTree.GetTankTypeSprite(tankNode.Tank.TankSO.TankType), _techTree.TankTierNumber[0], tankNode.Tank.ID, false, () =>
                            {
                                _tankInformation.SetActive(true);
                                var topUI = _tankInformationPanel.transform.GetChild(0);
                                topUI.GetChild(0).GetComponent<Image>().sprite = _techTree.GetTankTypeSprite(tankInfo.TankSO.TankType);
                                topUI.GetChild(1).GetComponent<Text>().text = _techTree.TankTierNumber[0];
                                topUI.GetChild(2).GetComponent<Text>().text = tankInfo.ID;

                                // 탱크 이미지 없으니까 일단  null
                                _tankInformationPanel.transform.GetChild(1).GetComponent<Image>().sprite = null;

                                _tankInformationPanel.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                                _tankInformationPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() =>
                                {
                                    FindObjectOfType<TankModelManager>().ChangeTankModel(tankInfo);
                                    _tankInformation.SetActive(false);
                                });

                                _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
                                _tankInformationPanel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() =>
                                {
                                    TechTreeDataManager.AddTank(_techTree.TechTreeSO[index].CountryType, tankInfo.ID);
                                    _tankInformation.SetActive(false);
                                });

                                _tankInformationPanel.transform.GetChild(5).GetComponent<Button>().onClick.RemoveAllListeners();
                                _tankInformationPanel.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() =>
                                {
                                    _tankInformation.SetActive(false);
                                });
                            });
                            tankNodeConnectHorizontalLineUI.SetActive(true);

                            if (tankNode.ChildTankNode.Length > 0)
                            {
                                tankNode = tankNode.ChildTankNode[0];
                            }
                            else
                            {
                                tankNode = null;
                            }
                        }

                        Destroy(tankNodeConnectHorizontalLineUI);
                        tankNodeRowTemplate.SetActive(true);
                    }
                }
            });

            countryToggle.gameObject.SetActive(true);
            countryToggle.isOn = index == 0;
        }
    }

    public override void OnOpenEvents()
    {
        base.OnOpenEvents();

        int idx = 1;
        _startSequence = DOTween.Sequence()
        .PrependCallback(() =>
        {
            _techTreeScrollView.anchoredPosition = Vector2.right * 1000f;

            idx = 1;
            foreach (RectTransform rect in _toggleList)
            {
                rect.anchoredPosition += Vector2.down * 100f * idx++;
            }
        })
        .Append(_techTreeScrollView.DOAnchorPosX(0f, 1f))
        .InsertCallback(0.5f, () =>
        {
            idx = 1;
            foreach (RectTransform rect in _toggleList)
            {
                rect.DOAnchorPosY(-25f, 0.2f * idx++);
            }
        })
        .AppendCallback(() => _scrollRect.normalizedPosition = Vector2.zero);

        _tankInformation.SetActive(false);
    }

    public override void OnBackButton()
    {
        _tankInformation.SetActive(false);
        base.OnBackButton();
    }

    public void OnItemButton()
    {
        _tankInformation.SetActive(false);
        CanvasManager.ChangeCanvas(CanvasType.Gear, CanvasType);
    }
}
