using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCanvas : MonoBehaviour
{
    private Canvas _canvas;

    private void Awake()
    {
        TryGetComponent(out _canvas);
    }

    /// <summary> 임시로 만든 함수(클래스) 싹다 지워도됨 </summary>
    public void OnBackButton()
    {
        Time.timeScale = 1f;
        _canvas.enabled = false;
    }
}
