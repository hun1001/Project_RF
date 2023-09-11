using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;

public class ShellToggleGroupSATManager : ToggleGroup
{
    [SerializeField]
    private Toggle _templateToggle = null;

    [SerializeField]
    private SATInformationHandle _satInformationHandle = null;

    private List<ShellTemplateHandle> _templateList = new List<ShellTemplateHandle>();
    public List<ShellTemplateHandle> TemplateList => _templateList;

    public void AddToggle(int shellNumber, string shellName, Sprite shellSprite, UnityAction<bool> unityAction)
    {
        var toggleHandle = Instantiate(_templateToggle, transform).GetComponent<ShellTemplateHandle>();
        toggleHandle.gameObject.SetActive(true);

        toggleHandle.SetShellTemplate(shellNumber, shellName, shellSprite, unityAction);
        _templateList.Add(toggleHandle);
    }

    public void SetCoolDown(float coolDown)
    {
        foreach (var template in _templateList)
        {
            template.CoolDownBarHandle.SetCoolDown(coolDown);
        }
    }

    public void SetSAT(BaseSubArmament sat)
    {
        _satInformationHandle.Setting(sat);
        transform.SetParent(transform.parent);
    }
}
