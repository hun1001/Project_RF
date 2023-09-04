using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SATToggleTamplateHandle : MonoBehaviour
{
    [SerializeField]
    private Text _satText = null;

    public void SetText(string text)
    {
        _satText.text = text;
    }
}
