using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void SetText(string value)
    {
        _text.text = value;
    }

    public void SetText(int value) => SetText(value.ToString());
    public void SetText(float value) => SetText(value.ToString());
}
