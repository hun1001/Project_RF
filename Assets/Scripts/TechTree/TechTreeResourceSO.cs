using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TechTreeResourceSO", menuName = "ScriptableObjects/TechTreeResourceSO", order = 1)]
public class TechTreeResourceSO : ScriptableObject
{
    public readonly string[] TankTierNumber = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX" };

    [SerializeField]
    private Sprite[] _tankTypeSprites = null;
    public Sprite[] TankTypeSprites => _tankTypeSprites;
}
