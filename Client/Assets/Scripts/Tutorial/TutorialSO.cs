using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Tutorial")]
public class TutorialSO : ScriptableObject
{
    [TextArea(3, 5)]
    public string[] TutorialTexts;
}
