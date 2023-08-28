using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class TechTreeCountryToggles : MonoBehaviour
{
    [SerializeField]
    private ToggleGroupManager _toggleGroupManager = null;

    [SerializeField]
    private GameObject _countryToggleTemplate = null;

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
    }

    public void AddCountryToggleAction(int idx, UnityAction onValueChangedToTrue)
    {
        _toggleGroupManager.transform.GetChild(idx + 1).GetComponent<Toggle>().onValueChanged.AddListener((bool value) =>
        {
            if (value)
            {
                onValueChangedToTrue?.Invoke();
            }
        });
    }

    public void ChangeFirstToggleValue(bool value)
    {
        _toggleGroupManager.transform.GetChild(1).GetComponent<Toggle>().isOn = value;
    }
}
