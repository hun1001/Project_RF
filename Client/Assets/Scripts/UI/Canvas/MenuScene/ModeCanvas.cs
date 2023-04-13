using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ModeCanvas : BaseCanvas
{
    [Header("Modes")]
    [SerializeField]
    private RectTransform _modesFrame;
    private Image[] _modeImages;

    [Header("Maps")]
    [SerializeField]
    private RectTransform _content;
    [SerializeField]
    private RectTransform _scrollView;

    private Image[] _mapImages;

    private void Awake()
    {
        _mapImages = _content.GetComponentsInChildren<Image>();
        _modeImages = _modesFrame.GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        _startSequence = DOTween.Sequence()
        .SetAutoKill(false)
        .PrependCallback(() =>
        {
            foreach(Image image in _modeImages)
            {
                image.DOFade(0f, 0f);
            }
        })
        .Prepend(_modesFrame.DOAnchorPosX(-200f, 0f))
        .Append(_modesFrame.DOAnchorPosX(48f, 0.5f))
        .InsertCallback(0.25f, () =>
        { 
            foreach(Image image in _modeImages)
            {
                image.DOFade(1f, 0.5f);
            }
        })
        .PrependCallback(() =>
        {
            _scrollView.anchoredPosition += Vector2.right * 600f;
            foreach (Image image in _mapImages)
            {
                image.DOFade(0.3f, 0f);
            }
        })
        .Append(_scrollView.DOAnchorPosX(0f, 1.2f))
        .InsertCallback(0.7f, () =>
        {
            foreach (Image image in _mapImages)
            {
                image.DOFade(1f, 1f);
            }
        });
    }
}
