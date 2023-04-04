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

    private void Awake()
    {
        _tankInformationPanel.SetActive(false);

        _countryToggleTemplate = _countryToggleGroupManager.transform.GetChild(0).gameObject;
        _tankNodeRowTemplate = _tankNodeContentTransform.GetChild(0).gameObject;
        _tankNodeTemplate = _tankNodeRowTemplate.transform.GetChild(0).gameObject;

        _countryToggleTemplate.SetActive(false);
        _tankNodeRowTemplate.SetActive(false);
        _tankNodeTemplate.SetActive(false);

        for (int i = 0; i < _techTree.TechTreeSO.Length; ++i)
        {
            int index = i;

            var countryToggle = Instantiate(_countryToggleTemplate, _countryToggleGroupManager.transform).GetComponent<Toggle>();
            countryToggle.transform.GetChild(0).GetComponent<Image>().sprite = _techTree.TechTreeSO[index].CountrySprite;

            countryToggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    for (int k = 1; k < _tankNodeContentTransform.transform.childCount; ++k)
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
                            _techTree.GetTankTypeSprite(_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.TankType);

                            var eventTrigger = tankNode.GetComponent<EventTrigger>();
                            var entry = new EventTrigger.Entry();
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((eventData) =>
                            {
                                Debug.Log(_techTree.TechTreeSO[index][jIndex, lIndex].ID);
                                _tankInformationPanel.SetActive(true);
                                var topUI = _tankInformationPanel.transform.GetChild(0);
                                topUI.GetChild(0).GetComponent<Image>().sprite = _techTree.GetTankTypeSprite(_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.TankType);
                                topUI.GetChild(1).GetComponent<Text>().text = _techTree.TankTierNumber[lIndex];
                                topUI.GetChild(2).GetComponent<Text>().text = _techTree.TechTreeSO[index][jIndex, lIndex].ID;

                                // 탱크 이미지 없으니까 일단  null
                                _tankInformationPanel.transform.GetChild(1).GetComponent<Image>().sprite = null;
                            });
                            eventTrigger.triggers.Add(entry);

                            eventTrigger.enabled = true;

                            tankNode.transform.GetChild(1).GetComponent<Text>().text = _techTree.TankTierNumber[lIndex];
                            tankNode.transform.GetChild(2).GetComponent<Text>().text = _techTree.TechTreeSO[index][jIndex, lIndex].ID;

                            tankNode.GetComponent<Image>().enabled = true;

                            tankNode.SetActive(true);
                        }

                        rowTransform.GetComponent<HorizontalLayoutGroup>().enabled = true;
                        rowTransform.GetComponent<ContentSizeFitter>().enabled = true;
                        rowTransform.gameObject.SetActive(true);

                        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)rowTransform.transform);
                    }
                }
            });

            countryToggle.onValueChanged.Invoke(index == 0);
            countryToggle.gameObject.SetActive(true);
        }

    }
}
