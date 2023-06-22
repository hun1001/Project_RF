using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextController : MonoBehaviour, IText
{
    [SerializeField]
    private TextMeshProUGUI _text;

    private void Awake()
    {
        if (_text == null) _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetText(int value) => SetText(value.ToString());
    public void SetText(float value) => SetText(value.ToString());
}
