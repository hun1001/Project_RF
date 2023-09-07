using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TMP_Text))]
public class TextController : MonoBehaviour, IText
{
    [SerializeField]
    private TMP_Text _text;

    private Tweener _tweener;

    private void Awake()
    {
        if (_text == null) _text = GetComponent<TMP_Text>();
    }

    public void SetText(string text)
    {
        if (_tweener != null)
        {
            _tweener.Kill();
            _tweener = null;
        }

        if (text.Length > _text.maxVisibleCharacters) _text.maxVisibleCharacters = text.Length;

        _text.text = text;
    }

    private void TMPDOText(float duration)
    {
        _text.maxVisibleCharacters = 0;
        _tweener = DOTween.To(x => _text.maxVisibleCharacters = (int)x, 0f, _text.text.Length, duration).SetEase(Ease.Linear);
    }

    public void Typing(string text, float duration)
    {
        _text.text = text;
        TMPDOText(duration);
    }

    public void SetText(int value) => SetText(value.ToString());
    public void SetText(float value) => SetText(value.ToString());

    public void Typing(int value, float duration) => Typing(value.ToString(), duration);
    public void Typing(float value, float duration) => Typing(value.ToString(), duration);
}
