using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TechTreeCanvas : BaseCanvas
{
    [SerializeField]
    private TechTree _techTree = null;

    [SerializeField]
    private ToggleGroupManager _countryToggleGroupManager = null;

    [SerializeField]
    private Transform _tankNodeContentTransform = null;

    [SerializeField]
    private GameObject _tankInformationPanel = null;

    private GameObject _countryToggleTemplate = null;
    private GameObject _tankNodeRowTemplate = null;
    private GameObject _tankNodeTemplate = null;
    private GameObject _tankNodeConnectHorizontalLineTemplate = null;

    private GameObject _verticalLineRowTemplate = null;
    private GameObject _noneLineTemplate = null;
    private GameObject _verticalUpLineTemplate = null;
    private GameObject _verticalDownLineTemplate = null;

    private void Awake()
    {
        _tankInformationPanel.SetActive(false);

        _countryToggleTemplate = _countryToggleGroupManager.transform.GetChild(0).gameObject;
        _tankNodeRowTemplate = _tankNodeContentTransform.GetChild(0).gameObject;
        _tankNodeTemplate = _tankNodeRowTemplate.transform.GetChild(0).gameObject;
        _tankNodeConnectHorizontalLineTemplate = _tankNodeRowTemplate.transform.GetChild(1).gameObject;

        _verticalLineRowTemplate = _tankNodeContentTransform.GetChild(1).gameObject;
        _noneLineTemplate = _verticalLineRowTemplate.transform.GetChild(0).gameObject;
        _verticalUpLineTemplate = _verticalLineRowTemplate.transform.GetChild(1).gameObject;
        _verticalDownLineTemplate = _verticalLineRowTemplate.transform.GetChild(2).gameObject;

        _countryToggleTemplate.SetActive(false);
        _tankNodeRowTemplate.SetActive(false);
        _tankNodeTemplate.SetActive(false);
        _tankNodeConnectHorizontalLineTemplate.SetActive(false);

        _verticalLineRowTemplate.SetActive(false);
        _noneLineTemplate.SetActive(false);
        _verticalUpLineTemplate.SetActive(false);
        _verticalDownLineTemplate.SetActive(false);

        for (int i = 0; i < _techTree.TechTreeSO.Length; ++i)
        {
            int index = i;

            var countryToggle = Instantiate(_countryToggleTemplate, _countryToggleGroupManager.transform).GetComponent<Toggle>();
            countryToggle.transform.GetChild(0).GetComponent<Image>().sprite = _techTree.TechTreeSO[index].FlagSprite;

            countryToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
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
                            var tankNode = Instantiate(_tankNodeTemplate, rowTransform);

                            // 대충 색 설정 해야됨
                            tankNode.transform.GetChild(0).GetComponent<Image>().sprite = _techTree.GetTankTypeSprite(_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.TankType);

                            var eventTrigger = tankNode.GetComponent<EventTrigger>();
                            var entry = new EventTrigger.Entry();
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((eventData) =>
                            {
                                _tankInformationPanel.SetActive(true);
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
                                    FindObjectOfType<TankModelManager>().ChangeTankModel(_techTree.TechTreeSO[index][jIndex, lIndex]);
                                    _tankInformationPanel.SetActive(false);
                                });
                            });
                            eventTrigger.triggers.Add(entry);

                            eventTrigger.enabled = true;

                            tankNode.transform.GetChild(1).GetComponent<Text>().text = _techTree.TankTierNumber[lIndex];
                            tankNode.transform.GetChild(2).GetComponent<Text>().text = _techTree.TechTreeSO[index][jIndex, lIndex].ID;

                            tankNode.GetComponent<Image>().enabled = true;

                            tankNode.SetActive(true);

                            if (lIndex != _techTree.TechTreeSO[index].GetTankArrayLength(jIndex) - 1)
                            {
                                var tankNodeConnectLine = Instantiate(_tankNodeConnectHorizontalLineTemplate, rowTransform);
                                tankNodeConnectLine.SetActive(true);
                            }
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

        _countryToggleGroupManager.transform.GetChild(1).GetComponent<Toggle>().onValueChanged.Invoke(true);
        _countryToggleGroupManager.transform.GetChild(1).GetComponent<Toggle>().isOn = true;
    }

    public void OnBackButton()
    {
        _tankInformationPanel.SetActive(false);
        CanvasManager.ChangeCanvas(CanvasType.Menu);
    }
}
