using UnityEngine;
using Util;

public enum AnchorType
{
    Left,
    Right,
    Top,
    Bottom,
    LeftTop,
    RightTop,
    LeftBottom,
    RightBottom,
}

public class UIManager : Singleton<UIManager>
{
    public void RectTransformSetPos(RectTransform rect, Vector2 vector)
    {
        rect.anchoredPosition = vector;
    }
    public void RectTransformSetPos(RectTransform rect, float posX, float posY) => RectTransformSetPos(rect, new Vector2(posX, posY));

    public void RectTransformSetAnchor(RectTransform rect, AnchorType type)
    {
        switch (type)
        {
            case AnchorType.Left:
                {
                    rect.anchorMax = new Vector2(0f, 0.5f);
                    rect.anchorMin = new Vector2(0f, 0.5f);
                    break;
                }
            case AnchorType.Right:
                {
                    rect.anchorMax = new Vector2(1f, 0.5f);
                    rect.anchorMin = new Vector2(1f, 0.5f);
                    break;
                }
            case AnchorType.Top:
                {
                    rect.anchorMax = new Vector2(0.5f, 1f);
                    rect.anchorMin = new Vector2(0.5f, 1f);
                    break;
                }
            case AnchorType.Bottom:
                {
                    rect.anchorMax = new Vector2(0.5f, 0f);
                    rect.anchorMin = new Vector2(0.5f, 0f);
                    break;
                }
            case AnchorType.LeftTop:
                {
                    rect.anchorMax = new Vector2(0f, 1f);
                    rect.anchorMin = new Vector2(0f, 1f);
                    break;
                }
            case AnchorType.RightTop:
                {
                    rect.anchorMax = new Vector2(1f, 1f);
                    rect.anchorMin = new Vector2(1f, 1f);
                    break;
                }
            case AnchorType.LeftBottom:
                {
                    rect.anchorMax = new Vector2(0f, 0f);
                    rect.anchorMin = new Vector2(0f, 0f);
                    break;
                }
            case AnchorType.RightBottom:
                {
                    rect.anchorMax = new Vector2(1f, 0f);
                    rect.anchorMin = new Vector2(1f, 0f);
                    break;
                }
        }
    }

    public void RectTransformSetPivot(RectTransform rect, Vector2 vector)
    {
        rect.pivot = vector;
    }
    public void RectTransformSetPivot(RectTransform rect, float pivotX = 0.5f, float pivotY = 0.5f) => RectTransformSetPivot(rect, new Vector2(pivotX, pivotY));

    public void RectTransformSetSize(RectTransform rect, Vector2 vector)
    {
        rect.sizeDelta = vector;
    }
    public void RectTransformSetSize(RectTransform rect, float sizeX, float sizeY) => RectTransformSetSize(rect, new Vector2(sizeX, sizeY));
}
