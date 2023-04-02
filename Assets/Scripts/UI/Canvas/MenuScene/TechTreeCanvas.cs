using System;
using System.Collections;
using System.Collections.Generic;
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


    private GameObject _techTreeNodeTemplate = null;

    private void Awake()
    {
        _techTreeNodeTemplate = _techTreeContentTransform.GetChild(0).gameObject;
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
                    for (int k = 0; k < techTreeSO.GetTankArrayLength(j); ++k)
                    {
                        Debug.Log(techTreeSO[j, k].ToString());
                        var techTreeNode = Instantiate(_techTreeNodeTemplate, _techTreeContentTransform);

                        // 눈에 보이는 정보들 초기화 해주는 부분
                        techTreeNode.GetComponent<Image>().enabled = true;

                        // 클릭시 일어나는 이벤트 추가해주는 부분
                        var techTreeNodeEventTrigger = techTreeNode.GetComponent<EventTrigger>();
                        var entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerClick;
                        entry.callback.AddListener((eventData) =>
                        {
                            Debug.Log("Click");
                        });
                        techTreeNodeEventTrigger.triggers.Add(entry);

                        techTreeNode.SetActive(true);
                    }
                }
            });
        }
        _countryDropdown.onValueChanged.Invoke(0);
    }
}
