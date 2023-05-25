using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextController : MonoBehaviour, IText
{
    [SerializeField]
    private Text _text;

    private void Awake()
    {
        if (_text == null) _text = GetComponent<Text>();
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetText(int value) => SetText(value.ToString());
    public void SetText(float value) => SetText(value.ToString());
}
