using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TechTree", menuName = "SO/TechTree")]
public class TechTreeSO : ScriptableObject
{
    [SerializeField]
    private CountryType _countryType = CountryType.None;
    public CountryType CountryType
    {
        get => _countryType;
        set => _countryType = value;
    }

    [SerializeField]
    private Sprite _flagSprite = null;
    public Sprite FlagSprite
    {
        get => _flagSprite;
        set => _flagSprite = value;
    }

    [SerializeField]
    private TankArray[] _tankArrays = null;
    public Tank this[int i, int j] => _tankArrays[i][j];

    public int Length => _tankArrays.Length;
    public int GetTankArrayLength(int i) => _tankArrays[i].Length;

    public void SetTankArray(List<List<Tank>> tankList)
    {
        _tankArrays = new TankArray[tankList.Count];
        for (int i = 0; i < tankList.Count; i++)
        {
            _tankArrays[i] = new TankArray(tankList[i].ToArray());
        }
    }

    [Serializable]
    public struct TankArray
    {
        [SerializeField]
        private Tank[] _tanks;
        public Tank this[int index] => _tanks[index];
        public int Length => _tanks.Length;

        public TankArray(Tank[] tanks)
        {
            _tanks = tanks;
        }
    }
}
