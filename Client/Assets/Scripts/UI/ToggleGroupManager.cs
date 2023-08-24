using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ToggleGroupManager : ToggleGroup
{
    [SerializeField]
    private Toggle _templateToggle = null;

    private List<Toggle> _toggleList = new List<Toggle>();
    public List<Toggle> ToggleList => _toggleList;

    public void AddToggle(int shellNumber, string shellName, Sprite shellSprite, UnityAction<bool> unityAction)
    {
        var toggleHandle = Instantiate(_templateToggle, transform).GetComponent<ShellTemplateHandle>();
        toggleHandle.gameObject.SetActive(true);

        toggleHandle.SetShellTemplate(shellNumber, shellName, shellSprite, unityAction);
        _toggleList.Add(toggleHandle.Toggle);
    }
}
