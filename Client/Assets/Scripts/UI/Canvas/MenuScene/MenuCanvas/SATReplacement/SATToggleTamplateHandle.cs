using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SATToggleTamplateHandle : MonoBehaviour
{
    [SerializeField]
    private Text _satText = null;

    [SerializeField]
    private Toggle _satToggle = null;

    public void Setting(string text)
    {
        _satText.text = text;


    }
}
