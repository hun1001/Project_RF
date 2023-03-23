using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Tank : CustomObject
{
    [SerializeField]
    private TankSO _tankSO = null;

    private TankSO _thisTankSO = null;
    public TankSO TankData => _thisTankSO;

    [SerializeField]
    private SoundBoxSO _tankSound = null;
    public SoundBoxSO TankSound => _tankSound;

    private GroupType _groupType = GroupType.None;
    public GroupType GroupType => _groupType;

    private Turret _turret = null;
    public Turret Turret => _turret;

    public Tank SetGroupType(GroupType groupType)
    {
        _groupType = groupType;
        return this;
    }

    protected override void Awake()
    {
        base.Awake();
        _turret = GetComponent<Turret>();
        _thisTankSO = _tankSO.Clone();
    }
}
