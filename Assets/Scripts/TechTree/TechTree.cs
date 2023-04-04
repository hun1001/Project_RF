using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TechTree : MonoBehaviour
{
    [SerializeField]
    private TechTreeSO[] _techTreeSO = null;
    public TechTreeSO[] TechTreeSO => _techTreeSO;

    [SerializeField]
    private Sprite[] _tankTypeSprites = null;
    public Sprite[] TankTypeSprites => _tankTypeSprites;

    public readonly string[] TankTierNumber = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
}
