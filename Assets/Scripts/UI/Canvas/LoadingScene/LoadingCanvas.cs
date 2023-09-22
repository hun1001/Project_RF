using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
    [SerializeField]
    private Text _tipText = null;

    private void Start()
    {
        SetRandomTipText();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetRandomTipText();
        }
    }

    private void SetRandomTipText()
    {
        _tipText.text = TipManager.Instance.GetRandomTipText();
    }
}
