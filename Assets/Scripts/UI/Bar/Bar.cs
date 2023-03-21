using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    private Image _valueImage = null;
    private Image _beforeValueImage = null;

    private void Awake()
    {
        _valueImage = transform.GetChild(0).GetComponent<Image>();
        _valueImage = transform.GetChild(1).GetComponent<Image>();
    }

    public void ChangeValue(float value)
    {

    }
}
