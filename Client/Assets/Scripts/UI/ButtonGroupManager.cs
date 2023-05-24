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

    public void SetButton(int index, UnityAction action, Sprite image = null, bool interactable = true)
    {
        if (image != null)
        {
            _buttons[index].GetComponent<Image>().enabled = true;
            _buttons[index].GetComponent<Image>().sprite = image;
        }
        else
        {
            _buttons[index].GetComponent<Image>().enabled = false;
        }

        _buttons[index].interactable = interactable;

        _buttons[index]?.onClick.RemoveAllListeners();
        _buttons[index].onClick.AddListener(action);
    }
}
