using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PopupText : MonoBehaviour, IPoolReset
{
    [SerializeField]
    private Text _text;

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void PoolObjectReset()
    {
        _text.text = null;
    }

    public void SetText(int text) => SetText(text.ToString());
    public void SetText(float text) => SetText(text.ToString());
}
