using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCanvas : BaseCanvas
{
    [SerializeField]
    private Button _backButton = null;

    private void Awake()
    {
        _backButton.onClick.AddListener(() =>
        {
            CanvasManager.ChangeCanvas(CanvasType.Controller);
        });
    }
}
