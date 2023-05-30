using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Addressable;
using System.Reflection;

public class TechTreeCanvas : BaseCanvas
{
    [SerializeField]
    private TechTree _techTree = null;

    [SerializeField]
    private ToggleGroupManager _countryToggleGroupManager = null;

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

    private GameObject _verticalLineRowTemplate = null;
    private GameObject _noneLineTemplate = null;
    private GameObject _verticalUpLineTemplate = null;
    private GameObject _verticalDownLineTemplate = null;

    private List<RectTransform> _toggleList = new List<RectTransform>();

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

        _countryToggleTemplate = _countryToggleGroupManager.transform.GetChild(0).gameObject;

        _tankNodeRowTemplate = _tankNodeContentTransform.GetChild(0).gameObject;
        _tankNodeTemplate = _tankNodeRowTemplate.transform.GetChild(0).gameObject;
        _tankNodeConnectHorizontalLineTemplate = _tankNodeRowTemplate.transform.GetChild(1).gameObject;
        _tankNodeNullTemplate = _tankNodeRowTemplate.transform.GetChild(2).gameObject;
        _tankNodeConnectHorizontalNullLineTemplate = _tankNodeRowTemplate.transform.GetChild(3).gameObject;

        _verticalLineRowTemplate = _tankNodeContentTransform.GetChild(1).gameObject;
        _noneLineTemplate = _verticalLineRowTemplate.transform.GetChild(0).gameObject;
        _verticalUpLineTemplate = _verticalLineRowTemplate.transform.GetChild(1).gameObject;
        _verticalDownLineTemplate = _verticalLineRowTemplate.transform.GetChild(2).gameObject;

        _countryToggleTemplate.SetActive(false);
        _tankNodeRowTemplate.SetActive(false);
        _tankNodeTemplate.SetActive(false);
        _tankNodeConnectHorizontalLineTemplate.SetActive(false);
        _tankNodeNullTemplate.SetActive(false);
        _tankNodeConnectHorizontalNullLineTemplate.SetActive(false);

        _verticalLineRowTemplate.SetActive(false);
        _noneLineTemplate.SetActive(false);
        _verticalUpLineTemplate.SetActive(false);
        _verticalDownLineTemplate.SetActive(false);

        _techTreeScrollView.TryGetComponent(out _scrollRect);

        for (int i = 0; i < _techTree.TechTreeSO.Length; ++i)
        {
            int index = i;
            var countryToggle = Instantiate(_countryToggleTemplate, _countryToggleGroupManager.transform).GetComponent<Toggle>();
            countryToggle.transform.GetChild(0).GetComponent<Image>().sprite = _techTree.TechTreeSO[index].FlagSprite;
            _toggleList.Add(countryToggle.transform as RectTransform);

            countryToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    PlayButtonSound();
                    for (int k = 2; k < _tankNodeContentTransform.transform.childCount; ++k)
                    {
                        Destroy(_tankNodeContentTransform.transform.GetChild(k).gameObject);
                    }

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

                                    bool canUnlock;
                                    if (lIndex != 0)
                                    {
                                        if (_techTree.TechTreeSO[index][jIndex, lIndex - 1] != null)
                                        {
                                            canUnlock = TechTreeDataManager.GetTechTreeProgress(_techTree.TechTreeSO[index].CountryType)._tankProgressList.Contains(_techTree.TechTreeSO[index][jIndex, lIndex - 1].ID);
                                        }
                                        else
                                        {
                                            if (jIndex == 0)
                                            {
                                                canUnlock = _techTree.TechTreeSO[index].IsLink(jIndex + 1, lIndex - 1) == TechTreeLinkStateType.UpLink && TechTreeDataManager.GetTechTreeProgress(_techTree.TechTreeSO[index].CountryType)._tankProgressList.Contains(_techTree.TechTreeSO[index][jIndex + 1, lIndex - 1].ID);
                                            }
                                            else if (jIndex == _techTree.TechTreeSO[index].Length - 1)
                                            {
                                                canUnlock = TechTreeDataManager.GetTechTreeProgress(_techTree.TechTreeSO[index].CountryType)._tankProgressList.Contains(_techTree.TechTreeSO[index][jIndex - 1, _techTree.TechTreeSO[index].GetTankArrayLength(jIndex - 1) - 1].ID);
                                            }
                                            else
                                            {
                                                canUnlock = _techTree.TechTreeSO[index].IsLink(jIndex + 1, lIndex - 1) == TechTreeLinkStateType.UpLink && TechTreeDataManager.GetTechTreeProgress(_techTree.TechTreeSO[index].CountryType)._tankProgressList.Contains(_techTree.TechTreeSO[index][jIndex + 1, lIndex - 1].ID) || _techTree.TechTreeSO[index].IsLink(jIndex - 1, lIndex - 1) == TechTreeLinkStateType.DownLink && TechTreeDataManager.GetTechTreeProgress(_techTree.TechTreeSO[index].CountryType)._tankProgressList.Contains(_techTree.TechTreeSO[index][jIndex - 1, lIndex - 1].ID);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        canUnlock = true;
                                    }

                                    canUnlock = true;

                                    _tankInformation.SetActive(canUnlock);
                                    var topUI = _tankInformationPanel.transform.GetChild(0);
                                    topUI.GetChild(0).GetComponent<Image>().sprite = _techTree.GetTankTypeSprite(_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.TankType);
                                    topUI.GetChild(1).GetComponent<Text>().text = _techTree.TankTierNumber[lIndex];
                                    topUI.GetChild(2).GetComponent<Text>().text = _techTree.TechTreeSO[index][jIndex, lIndex].ID;

                                    // 탱크 이미지 없으니까 일단  null
                                    _tankInformationPanel.transform.GetChild(1).GetComponent<Image>().sprite = null;
                                    //_tankInformationPanel.transform.GetChild(2).GetComponent<Text>().text = $"SPEED: {_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.MaxSpeed}km/h\nReload: {_techTree.TechTreeSO[index][jIndex, lIndex].Turret.TurretStatSO}";

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

                        if (jIndex < _techTree.TechTreeSO[index].Length - 1)
                        {
                            var verticalLineRow = Instantiate(_verticalLineRowTemplate, _tankNodeContentTransform).transform;

                            for (int m = 0; m < _techTree.TechTreeSO[index].GetIsLinkLength(jIndex); ++m)
                            {
                                int mIndex = m;
                                GameObject line = null;

                                switch (_techTree.TechTreeSO[index].IsLink(jIndex, mIndex))
                                {
                                    case TechTreeLinkStateType.None:
                                        line = Instantiate(_noneLineTemplate, verticalLineRow);
                                        break;
                                    case TechTreeLinkStateType.UpLink:
                                        line = Instantiate(_verticalUpLineTemplate, verticalLineRow);

                                        break;
                                    case TechTreeLinkStateType.DownLink:
                                        line = Instantiate(_verticalDownLineTemplate, verticalLineRow);
                                        break;
                                    default:
                                        Debug.LogError("TechTreeLinkStateType Error");
                                        break;
                                }
                                line.SetActive(true);
                            }
                            verticalLineRow.gameObject.SetActive(true);
                        }

                        rowTransform.GetComponent<HorizontalLayoutGroup>().enabled = true;
                        rowTransform.GetComponent<ContentSizeFitter>().enabled = true;
                        rowTransform.gameObject.SetActive(true);

                        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)rowTransform.transform);
                    }
                }
            });

            countryToggle.gameObject.SetActive(true);
        }

        //_countryToggleGroupManager.transform.GetChild(1).GetComponent<Toggle>().onValueChanged.Invoke(true);
        _countryToggleGroupManager.transform.GetChild(1).GetComponent<Toggle>().isOn = true;
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
}
