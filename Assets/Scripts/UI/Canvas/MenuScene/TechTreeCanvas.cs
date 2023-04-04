using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TechTreeCanvas : BaseCanvas
{
    [SerializeField]
    private TechTree _techTree = null;

    [SerializeField]
    private Dropdown _countryDropdown = null;

    [SerializeField]
    private Transform _techTreeContentTransform = null;

    [SerializeField]
    private GameObject _tankInformationPanel = null;

    private GameObject _techTreeColumnTemplate = null;
    private GameObject _techTreeNodeTemplate = null;

    private void Awake()
    {
        _tankInformationPanel.SetActive(false);

        _techTreeColumnTemplate = _techTreeContentTransform.GetChild(0).gameObject;
        _techTreeNodeTemplate = _techTreeColumnTemplate.transform.GetChild(0).gameObject;
        _techTreeColumnTemplate.SetActive(false);
        _techTreeNodeTemplate.SetActive(false);

        _countryDropdown.options.Clear();

        foreach (var techTreeSO in _techTree.TechTreeSO)
        {
            _countryDropdown.options.Add(new Dropdown.OptionData(techTreeSO.CountryType.ToString()));
            _countryDropdown.onValueChanged.AddListener((i) =>
            {
                // UI 생성 PoolManager사용하게 변경 예정
                foreach (Transform child in _techTreeContentTransform)
                {
                    Destroy(child.gameObject);
                }

                for (int j = 0; j < techTreeSO.Length; ++j)
                {
                    int jIndex = j;
                    var techTreeColumn = Instantiate(_techTreeColumnTemplate, _techTreeContentTransform);
                    techTreeColumn.GetComponent<VerticalLayoutGroup>().enabled = true;
                    techTreeColumn.GetComponent<ContentSizeFitter>().enabled = true;
                    techTreeColumn.SetActive(true);
                    for (int k = 0; k < techTreeSO.GetTankArrayLength(jIndex); ++k)
                    {
                        int kIndex = k;
                        var techTreeNode = Instantiate(_techTreeNodeTemplate, techTreeColumn.transform);

                        // 눈에 보이는 정보들 초기화 해주는 부분
                        techTreeNode.transform.GetChild(1).GetComponent<Text>().text = techTreeSO[jIndex, kIndex].ID;
                        techTreeNode.GetComponent<Image>().enabled = true;

                        // 클릭시 일어나는 이벤트 추가해주는 부분
                        var techTreeNodeEventTrigger = techTreeNode.GetComponent<EventTrigger>();
                        var entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerClick;
                        entry.callback.AddListener((eventData) =>
                        {
                            _tankInformationPanel.transform.GetChild(1).GetComponent<Text>().text = techTreeSO[jIndex, kIndex].ID;
                            _tankInformationPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                            {
                                //TankModelManager.Instance.ChangeTankModel(techTreeSO[jIndex, kIndex]);
                                FindObjectOfType<TankModelManager>().ChangeTankModel(techTreeSO[jIndex, kIndex]);

                                _tankInformationPanel.SetActive(false);
                                CanvasManager.ChangeCanvas(CanvasType.Menu);
                            });
                            _tankInformationPanel.SetActive(true);
                        });
                        techTreeNodeEventTrigger.triggers.Add(entry);

                        techTreeNodeEventTrigger.enabled = true;

                        techTreeNode.SetActive(true);
                    }
                }
            });
        }
        _countryDropdown.onValueChanged.Invoke(0);
    }
}
