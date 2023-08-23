using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class ToggleGroupManager : ToggleGroup
{
    [SerializeField]
    private Toggle _templateToggle = null;

    public void AddToggle(int shellNumber, string shellName, Sprite shellSprite, UnityAction<bool> unityAction)
    {
        var toggle = Instantiate(_templateToggle, transform).GetComponent<ShellTemplateHandle>();
        toggle.gameObject.SetActive(true);

        toggle.SetShellTemplate(shellNumber, shellName, shellSprite, unityAction);
    }
}
