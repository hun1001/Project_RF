using UnityEngine.UI;
using UnityEngine.Events;

public class ToggleGroupManager : ToggleGroup
{
    private static Toggle _templateToggle = null;

    public void SetToggleGroup(string[] toggleText, UnityAction<bool>[] onValueChanged, int onIndex = 0)
    {
        _templateToggle ??= transform.GetChild(0).GetComponent<Toggle>();
        _templateToggle.gameObject.SetActive(false);

        for (int i = 0; i < toggleText.Length; ++i)
        {
            Toggle toggle = null;

            toggle = Instantiate(_templateToggle, transform);
            toggle.gameObject.SetActive(true);

            toggle.transform.GetChild(1).GetComponent<Text>().text = toggleText[i];
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(onValueChanged[i]);
            toggle.group = this;
            toggle.isOn = i == onIndex;
        }
    }
}
