using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class TechTreeCountryToggles : MonoBehaviour
{
    [SerializeField]
    private ShellToggleGroupSATManager _toggleGroupManager = null;

    [SerializeField]
    private GameObject _countryToggleTemplate = null;

    private List<Toggle> _toggleList = new List<Toggle>();

    public void CreateCountryToggles(Sprite flagSprite, UnityAction onValueChangedToTrue)
    {
        var countryToggle = Instantiate(_countryToggleTemplate, _toggleGroupManager.transform).GetComponent<Toggle>();
        countryToggle.transform.GetChild(0).GetComponent<Image>().sprite = flagSprite;

        countryToggle.onValueChanged.AddListener((bool value) =>
        {
            if (value)
            {
                onValueChangedToTrue?.Invoke();
            }
        });

        countryToggle.gameObject.SetActive(true);
        _toggleList.Add(countryToggle);
    }

    public void ChangeToggleValue(int idx)
    {
        _toggleList[idx].isOn = true;
    }
}
