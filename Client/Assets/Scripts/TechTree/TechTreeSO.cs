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
    private TankNode[] _tankRootNodes = null;
    public TankNode[] RootTankNodes => _tankRootNodes;
}

[Serializable]
public class TankNode
{
    [SerializeField]
    private Tank _tank = null;
    public Tank Tank => _tank;

    [SerializeField]
    private TankNode[] _childTankNode = null;
    public TankNode[] ChildTankNode => _childTankNode;
}