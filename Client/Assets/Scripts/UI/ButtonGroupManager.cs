using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ButtonGroupManager : MonoBehaviour
{
    private List<Button> _buttons = new List<Button>();

    private void Awake()
    {
        _buttons.AddRange(GetComponentsInChildren<Button>());
    }
}
