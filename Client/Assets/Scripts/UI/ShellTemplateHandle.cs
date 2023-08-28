using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShellTemplateHandle : MonoBehaviour
{
    [SerializeField]
    private Image _shellImage = null;

    [SerializeField]
    private Text _shellNameText = null;

    [SerializeField]
    private Text _shellNumberText = null;

    [SerializeField]
    private Toggle _toggle = null;
    public Toggle Toggle => _toggle;

    public bool isOn
    {
        get => _toggle.isOn;
        set => _toggle.isOn = value;
    }

    [SerializeField]
    private CoolDownBarHandle _coolDownBarHandle = null;
    public CoolDownBarHandle CoolDownBarHandle => _coolDownBarHandle;

    public void SetShellTemplate(int shellNumber, string shellName, Sprite shellSprite, UnityAction<bool> action)
    {
        _shellNumberText.text = shellNumber.ToString();
        _shellNameText.text = shellName;
        _shellImage.sprite = shellSprite;
        _toggle.onValueChanged.AddListener(action);
    }
}
