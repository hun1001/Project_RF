using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class ButtonGroupManager : MonoBehaviour
{
    [SerializeField]
    private List<Button> _buttons = null;

    public void SetButton(int index, UnityAction action, bool interactable = true)
    {
        _buttons[index].interactable = interactable;
        _buttons[index].onClick.RemoveAllListeners();
        _buttons[index].onClick.AddListener(action);
    }
}
