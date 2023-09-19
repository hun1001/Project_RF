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

    public readonly string[] TankTierNumber = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX" };

    private void Awake()
    {
        if (TechTreeDataManager.HasTank(CountryType.USSR, "T-34") == false)
        {
            TechTreeDataManager.AddTank(CountryType.USSR, "T-34");
        }
    }

    public Sprite GetTankTypeSprite(TankType tankType)
    {
        Sprite sprite = null;

        switch (tankType)
        {
            case TankType.Light:
                sprite = _tankTypeSprites[0];
                break;
            case TankType.Medium:
                sprite = _tankTypeSprites[1];
                break;
            case TankType.Heavy:
                sprite = _tankTypeSprites[2];
                break;
            case TankType.Destroyer:
                sprite = _tankTypeSprites[3];
                break;
            default:
                Debug.LogError("TankType Error");
                break;
        }

        return sprite;
    }
}
