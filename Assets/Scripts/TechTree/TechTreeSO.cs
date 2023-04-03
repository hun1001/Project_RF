using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TechTree", menuName = "SO/TechTree")]
public class TechTreeSO : ScriptableObject
{
    [SerializeField]
    private CountryType _countryType = CountryType.None;
    public CountryType CountryType => _countryType;

    [SerializeField]
    private TankArray[] _tankArrays = null;
    public Tank this[int i, int j] => _tankArrays[i][j];

    public int Length => _tankArrays.Length;
    public int GetTankArrayLength(int i) => _tankArrays[i].Length;

    [Serializable]
    public struct TankArray
    {
        [SerializeField]
        private Tank[] _tanks;
        public Tank this[int index] => _tanks[index];

        public int Length => _tanks.Length;
    }
}
