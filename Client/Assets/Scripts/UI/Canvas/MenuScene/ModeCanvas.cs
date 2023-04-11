using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ModeCanvas : BaseCanvas
{
    [Header("Maps")]
    [SerializeField]
    private RectTransform _content;
    [SerializeField]
    private RectTransform _scrollView;

    private Image[] _mapImages;
    private Vector3 _originPos = Vector3.zero;
    private Sequence _startSequence;

    private void Awake()
    {
        _mapImages = _content.GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        _startSequence = DOTween.Sequence()
        .SetAutoKill(false)
        .PrependCallback(() =>
        {
            _scrollView.anchoredPosition += Vector2.right * 600f;
            foreach (Image image in _mapImages)
            {
                image.DOFade(0.3f, 0f);
            }
        })
        .Append(_scrollView.DOAnchorPosX(0f, 1.2f))
        .InsertCallback(0.2f, () =>
        {
            foreach (Image image in _mapImages)
            {
                image.DOFade(1f, 1f);
            }
        });
    }

    public override void OnOpenAnimation()
    {
        _startSequence.Restart();
    }
}
