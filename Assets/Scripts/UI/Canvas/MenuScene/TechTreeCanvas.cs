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

    private GameObject _countryToggleTemplate = null;
    private GameObject _tankNodeRowTemplate = null;
    private GameObject _tankNodeTemplate = null;

    private void Awake()
    {
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
                    for (int k = 0; k < _tankNodeContentTransform.transform.childCount; ++k)
                    {
                        Destroy(_tankNodeContentTransform.transform.GetChild(k).gameObject);
                    }

                    for (int j = 0; j < _techTree.TechTreeSO[index].Length; ++j)
                    {
                        int jIndex = j;
                        var rowTransform = Instantiate(_tankNodeRowTemplate, _tankNodeContentTransform).transform;
                        rowTransform.GetComponent<HorizontalLayoutGroup>().enabled = true;
                        rowTransform.GetComponent<ContentSizeFitter>().enabled = true;
                        rowTransform.gameObject.SetActive(true);

                        for (int l = 0; l < _techTree.TechTreeSO[index].GetTankArrayLength(jIndex); ++l)
                        {
                            int lIndex = l;
                            var tankNode = Instantiate(_tankNodeTemplate, rowTransform);


                            // 대충 색 설정 해야됨
                            switch (_techTree.TechTreeSO[index][jIndex, lIndex].TankSO.TankType)
                            {
                                case TankType.Light:
                                    tankNode.transform.GetChild(0).GetComponent<Image>().sprite = _techTree.TankTypeSprites[0];
                                    break;
                                case TankType.Medium:
                                    tankNode.transform.GetChild(0).GetComponent<Image>().sprite = _techTree.TankTypeSprites[1];
                                    break;
                                case TankType.Heavy:
                                    tankNode.transform.GetChild(0).GetComponent<Image>().sprite = _techTree.TankTypeSprites[2];
                                    break;
                                case TankType.Destroyer:
                                    tankNode.transform.GetChild(0).GetComponent<Image>().sprite = _techTree.TankTypeSprites[3];
                                    break;
                                default:
                                    Debug.LogError("TankType Error");
                                    break;
                            }

                            var eventTrigger = tankNode.GetComponent<EventTrigger>();
                            var entry = new EventTrigger.Entry();
                            entry.eventID = EventTriggerType.PointerClick;
                            entry.callback.AddListener((eventData) =>
                            {
                                Debug.Log(_techTree.TechTreeSO[index][jIndex, lIndex].ID);
                            });
                            eventTrigger.triggers.Add(entry);

                            eventTrigger.enabled = true;

                            tankNode.transform.GetChild(1).GetComponent<Text>().text = _techTree.TankTierNumber[lIndex];
                            tankNode.transform.GetChild(2).GetComponent<Text>().text = _techTree.TechTreeSO[index][jIndex, lIndex].ID;

                            tankNode.GetComponent<Image>().enabled = true;

                            tankNode.SetActive(true);
                        }
                    }
                }
            });

            countryToggle.gameObject.SetActive(true);
        }

        _countryToggleGroupManager.transform.GetChild(0).GetComponent<Toggle>().onValueChanged.Invoke(true);
    }
}
