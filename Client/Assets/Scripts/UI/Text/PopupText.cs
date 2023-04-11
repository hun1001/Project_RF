using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Pool;

public class PopupText : MonoBehaviour, IPoolReset
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private float _moveValue;
    [SerializeField]
    private float _moveDuration;

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void DoMoveText()
    {
        transform.DOMoveY(transform.position.y + _moveValue, _moveDuration).OnComplete(() => PoolManager.Pool("PopupDamage", gameObject));
    }

    public void PoolObjectReset()
    {
        _text.text = null;
    }

    public void SetText(int text) => SetText(text.ToString());
    public void SetText(float text) => SetText(text.ToString());
}
