using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextController : MonoBehaviour, IText
{
    [SerializeField]
    private TMP_Text _text;

    private void Awake()
    {
        if (_text == null) _text = GetComponent<TMP_Text>();
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetText(int value) => SetText(value.ToString());
    public void SetText(float value) => SetText(value.ToString());
}
