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

    [SerializeField]
    private TechTreeLinkStateTypeArray[] _isLink = null;
    public TechTreeLinkStateType IsLink(int i, int j) => _isLink[i][j];

    public int Length => _tankArrays.Length;
    public int GetTankArrayLength(int i) => _tankArrays[i].Length;
    public int GetIsLinkLength() => _isLink.Length;
    public int GetIsLinkLength(int i) => _isLink.Length == 0 ? 0 : _isLink[i].Length;

    public void SetTankArray(List<List<Tank>> tankList)
    {
        _tankArrays = new TankArray[tankList.Count];
        for (int i = 0; i < tankList.Count; i++)
        {
            _tankArrays[i] = new TankArray(tankList[i].ToArray());
        }
    }

    public void SetIsLink(TechTreeLinkStateType[][] isLink)
    {
        _isLink = new TechTreeLinkStateTypeArray[isLink.Length];
        for (int i = 0; i < isLink.Length; i++)
        {
            _isLink[i] = new TechTreeLinkStateTypeArray(isLink[i]);
        }
    }

    public void SetIsLink(List<List<TechTreeLinkStateType>> isLink)
    {
        _isLink = new TechTreeLinkStateTypeArray[isLink.Count];
        for (int i = 0; i < isLink.Count; i++)
        {
            _isLink[i] = new TechTreeLinkStateTypeArray(isLink[i].ToArray());
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

    [Serializable]
    public struct TechTreeLinkStateTypeArray
    {
        [SerializeField]
        private TechTreeLinkStateType[] _techTreeLinkStateTypes;
        public TechTreeLinkStateType this[int index] => index >= Length ? TechTreeLinkStateType.None : _techTreeLinkStateTypes[index];
        public int Length => _techTreeLinkStateTypes == null ? 0 : _techTreeLinkStateTypes.Length;

        public TechTreeLinkStateTypeArray(TechTreeLinkStateType[] techTreeLinkStateTypes)
        {
            _techTreeLinkStateTypes = techTreeLinkStateTypes;
        }
    }
}
