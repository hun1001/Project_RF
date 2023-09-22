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
            _buttons[index].transform.GetChild(1).gameObject.SetActive(true);
            _buttons[index].transform.GetChild(1).GetComponent<Image>().sprite = image;
        }
        else
        {
            _buttons[index].transform.GetChild(1).gameObject.SetActive(false);
        }

        _buttons[index].interactable = interactable;

        _buttons[index]?.onClick.RemoveAllListeners();
        _buttons[index].onClick.AddListener(action);
        _buttons[index].onClick.AddListener(() =>
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                int idx = i;
                _buttons[idx].transform.GetChild(0).gameObject.SetActive(false);
            }
            _buttons[index].transform.GetChild(0).gameObject.SetActive(true);
        });
    }
}
