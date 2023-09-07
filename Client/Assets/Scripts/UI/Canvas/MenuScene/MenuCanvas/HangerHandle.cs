using Addressable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pool;
using Event;

public class HangerHandle : MonoBehaviour
{
    [SerializeField]
    protected AudioClip _buttonSound = null;

    [SerializeField]
    private GameObject _hangerObject = null;
    [SerializeField]
    private Transform _hangerContent = null;
    [SerializeField]
    private GameObject _tankTemplate = null;
    [SerializeField]
    private Sprite[] _flagSprites = null;

    [Header("Filter")]
    [SerializeField]
    private Toggle[] _countryFilterToggles = null;
    private List<bool> _isCountryFilter = null;

    [SerializeField]
    private Toggle[] _tankTypeFilterToggles = null;
    private List<bool> _isTankTypeFilter = null;

    [SerializeField]
    private Toggle[] _tankTierFilterToggles = null;
    private List<bool> _isTankTierFilter = null;

    private Dictionary<string, GameObject> _hangerDict = new Dictionary<string, GameObject>();
    private Dictionary<CountryType, List<GameObject>> _countryHangerDataDict = new Dictionary<CountryType, List<GameObject>>();
    private List<GameObject> _hangerList = new List<GameObject>();

    private string _currentTankID => PlayerDataManager.Instance.GetPlayerTankID();

    public void Init()
    {
        _countryHangerDataDict.Add(CountryType.USSR, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.Germany, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.USA, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.Britain, new List<GameObject>());
        _countryHangerDataDict.Add(CountryType.France, new List<GameObject>());
    }

    public void OpenEvent()
    {
        _hangerObject.SetActive(false);
        _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
    }

    public void CurrentTankInfoUpdate()
    {
        _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(true);
    }

    public void OpenHanger()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void HangerUpdate()
    {
        TechTreeProgress ussrData = TechTreeDataManager.GetTechTreeProgress(CountryType.USSR);
        TechTreeProgress germanyData = TechTreeDataManager.GetTechTreeProgress(CountryType.Germany);
        TechTreeProgress usaData = TechTreeDataManager.GetTechTreeProgress(CountryType.USA);
        TechTreeProgress britainData = TechTreeDataManager.GetTechTreeProgress(CountryType.Britain);
        TechTreeProgress franceData = TechTreeDataManager.GetTechTreeProgress(CountryType.France);
        TechTree techTree = FindObjectOfType<TechTree>();

        foreach (var id in ussrData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.USSR);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.USSR].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }

        foreach (var id in germanyData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.Germany);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.Germany].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }

        foreach (var id in usaData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.USA);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.USA].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }

        foreach (var id in britainData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.Britain);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.Britain].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }

        foreach (var id in franceData._tankProgressList)
        {
            if (_hangerDict.ContainsKey(id)) continue;

            Tank tank = AddressablesManager.Instance.GetResource<GameObject>(id).GetComponent<Tank>();
            var a = Instantiate(_tankTemplate, _hangerContent);
            a.SetActive(true);
            a.name = id;
            a.GetComponent<Image>().sprite = GetFlagSprite(CountryType.France);
            a.transform.GetChild(0).GetComponent<TextController>().SetText(id);
            a.transform.GetChild(1).GetComponent<Image>().sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
            a.transform.GetChild(2).GetComponent<TextController>().SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);

            if (_currentTankID == id)
            {
                a.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                a.transform.GetChild(3).gameObject.SetActive(false);
            }

            _hangerDict.Add(id, a);
            _countryHangerDataDict[CountryType.France].Add(a);

            a.GetComponent<Button>().onClick.RemoveAllListeners();
            a.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayButtonSound();
                _hangerDict[_currentTankID].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<TankModelManager>().ChangeTankModel(tank);
                CurrentTankInfoUpdate();
                EventManager.TriggerEvent(EventKeyword.ShellReplacement);
            });
        }
    }

    public void HangerSort()
    {
        _hangerList.Clear();

        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.USSR]));
        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.Germany]));
        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.USA]));
        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.Britain]));
        _hangerList.AddRange(Sort(_countryHangerDataDict[CountryType.France]));

        HangerFilter();
        HangerChangeOrder();
    }

    public void HangerFilter()
    {
        int countryIndex = -1;
        int typeIndex = -1;
        int tierIndex = -1;

        if (_isCountryFilter.Contains(true))
        {
            countryIndex = _isCountryFilter.IndexOf(true);
        }
        if (_isTankTypeFilter.Contains(true))
        {
            typeIndex = _isTankTypeFilter.IndexOf(true);
        }
        if (_isTankTierFilter.Contains(true))
        {
            tierIndex = _isTankTierFilter.IndexOf(true);
        }

        // 모두 비활성화
        if (countryIndex < 0 && typeIndex < 0 && tierIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                _hangerList[i].SetActive(true);
            }
        }
        // 타입 필터
        else if (typeIndex >= 0 && countryIndex < 0 && tierIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.TankType == (TankType)typeIndex)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 나라 필터
        else if (countryIndex >= 0 && typeIndex < 0 && tierIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.CountryType == (CountryType)(countryIndex + 1))
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 티어 필터
        else if (tierIndex >= 0 && typeIndex < 0 && countryIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.TankTier == tierIndex + 1)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 나라, 타입 필터
        else if (countryIndex >= 0 && typeIndex >= 0 && tierIndex < 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.CountryType == (CountryType)(countryIndex + 1) && tank.TankSO.TankType == (TankType)typeIndex)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 나라, 티어 필터
        else if (countryIndex >= 0 && typeIndex < 0 && tierIndex >= 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.CountryType == (CountryType)(countryIndex + 1) && tank.TankSO.TankTier == tierIndex + 1)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 티어, 타입 필터
        else if (countryIndex < 0 && typeIndex >= 0 && tierIndex >= 0)
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.TankType == (TankType)typeIndex && tank.TankSO.TankTier == tierIndex + 1)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
        // 셋다 필터
        else
        {
            for (int i = 0; i < _hangerList.Count; i++)
            {
                Tank tank = AddressablesManager.Instance.GetResource<GameObject>(_hangerList[i].name).GetComponent<Tank>();

                if (tank.TankSO.CountryType == (CountryType)(countryIndex + 1) && tank.TankSO.TankType == (TankType)typeIndex && tank.TankSO.TankTier == tierIndex + 1)
                {
                    _hangerList[i].SetActive(true);
                }
                else
                {
                    _hangerList[i].SetActive(false);
                }
            }
        }
    }

    public void FilterInit()
    {
        _isCountryFilter = new List<bool>();
        for (int i = 0; i < _countryFilterToggles.Length; i++)
        {
            int index = i;
            _isCountryFilter.Add(false);
            _countryFilterToggles[index].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    PlayButtonSound();
                    _isCountryFilter[index] = true;
                }
                else
                {
                    _isCountryFilter[index] = false;
                }

                HangerFilter();
            });
        }

        _isTankTypeFilter = new List<bool>();
        for (int i = 0; i < _tankTypeFilterToggles.Length; i++)
        {
            int index = i;
            _isTankTypeFilter.Add(false);
            _tankTypeFilterToggles[index].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    PlayButtonSound();
                    _isTankTypeFilter[index] = true;
                }
                else
                {
                    _isTankTypeFilter[index] = false;
                }

                HangerFilter();
            });
        }

        _isTankTierFilter = new List<bool>();
        for (int i = 0; i < _tankTierFilterToggles.Length; i++)
        {
            int index = i;
            _isTankTierFilter.Add(false);
            _tankTierFilterToggles[index].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    PlayButtonSound();
                    _isTankTierFilter[index] = true;
                }
                else
                {
                    _isTankTierFilter[index] = false;
                }

                HangerFilter();
            });
        }
    }

    public List<GameObject> Sort(List<GameObject> list)
    {
        int gap = list.Count / 2;
        while (gap > 0)
        {
            for (int i = gap; i < list.Count; i++)
            {
                GameObject temp = list[i];
                int j = i;
                while (j >= gap && ShouldSwap(list[j - gap].name, temp.name))
                {
                    list[j] = list[j - gap];
                    j -= gap;
                }
                list[j] = temp;
            }
            gap /= 2;
        }

        return list;
    }

    public void HangerChangeOrder()
    {
        for (int i = 0; i < _hangerList.Count; i++)
        {
            GameObject item = _hangerList[i];

            item.transform.SetSiblingIndex(i);
        }
    }

    private bool ShouldSwap(string a, string b)
    {
        Tank tankA = AddressablesManager.Instance.GetResource<GameObject>(a).GetComponent<Tank>();
        Tank tankB = AddressablesManager.Instance.GetResource<GameObject>(b).GetComponent<Tank>();

        if (tankA != null && tankB != null)
        {
            // 같은 타입일 때만 비교하여 정렬
            if (tankA.TankSO.CountryType == tankB.TankSO.CountryType)
            {
                return tankA.TankSO.TankTier > tankB.TankSO.TankTier;
            }
        }

        return false;
    }

    private Sprite GetFlagSprite(CountryType type)
    {
        switch (type)
        {
            case CountryType.USSR:
                return _flagSprites[0];
            case CountryType.Germany:
                return _flagSprites[1];
            case CountryType.USA:
                return _flagSprites[2];
            case CountryType.Britain:
                return _flagSprites[3];
            case CountryType.France:
                return _flagSprites[4];
            default:
                return null;
        }
    }

    public void PlayButtonSound()
    {
        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", Vector3.zero, Quaternion.identity);
        audioSource.SetSound(_buttonSound);
        audioSource.SetGroup(AudioMixerType.Sfx);
        audioSource.SetVolume(1f);
        audioSource.Play();
    }
}
