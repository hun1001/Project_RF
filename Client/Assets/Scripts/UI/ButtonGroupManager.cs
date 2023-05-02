using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class ButtonGroupManager : MonoBehaviour
{
    [SerializeField]
    private List<Button> _buttons = null;
    [SerializeField]
    private List<FloatingJoystick> _joysticks = null;
    public List<FloatingJoystick> Joysticks => _joysticks;

    public void SetButton(int index, UnityAction action, bool interactable = true)
    {
        //_joysticks[0].enabled = false;
        _buttons[index].interactable = interactable;

        _buttons[index].onClick.RemoveAllListeners();

        _buttons[index].onClick.AddListener(action);
    }

    public void SetDragButton(int index, Action action, bool interactable = true)
    {
        _buttons[index].enabled = false;
        _joysticks[0].enabled = true;

        _buttons[index].onClick.RemoveAllListeners();

        _joysticks[0].AddOnPointerUpAction(action);
    }
}
