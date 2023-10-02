using System.Collections.Generic;
using System.Xml.Xsl;
using UnityEngine;

public class Tank : CustomObject, IPoolReset
{
    [SerializeField]
    private TankSO _tankSO = null;
    public TankSO TankSO
    {
        get => _tankSO;
        set => _tankSO = value;
    }

    private TankSO _thisTankSO = null;
    public TankSO TankData => _thisTankSO;

    [SerializeField]
    private SoundBoxSO _tankSound = null;
    public SoundBoxSO TankSound => _tankSound;

    private GroupType _groupType = GroupType.None;
    public GroupType GroupType => _groupType;

    private Turret _turret = null;
    public Turret Turret => _turret;

    public bool IsDead => _thisTankSO.HP <= 0;

    private BaseSubArmament _subArmament = null;
    public bool TryGetSecondaryArmament(out BaseSubArmament secondaryArmament)
    {
        secondaryArmament = _subArmament;
        return _hasSubArmament;
    }

    private bool _hasSubArmament = false;
    
    private Enhancement _enhancement = null;
    public Enhancement Enhancement => _enhancement;

    public Tank SetTank(GroupType groupType, BaseSubArmament secondaryArmament = null)
    {
        _hasSubArmament = secondaryArmament != null;
        Transform satTransform = null;
        if(Turret.SecondFirePoint != null)
        {
            satTransform = Turret.SecondFirePoint;
        }
        else
        {
            satTransform = Turret.FirePoint;
        }

        _subArmament = secondaryArmament?.Setting(this, satTransform);

        _subArmament?.transform.SetParent(this.transform);

        _groupType = groupType;

        _enhancement.Init();

        return this;
    }

    protected override void Awake()
    {
        base.Awake();
        _turret = GetComponent<Turret>();
        _thisTankSO = _tankSO.Clone();
        _enhancement = new Enhancement();
    }

    public void PoolObjectReset()
    {
        _thisTankSO = _tankSO.Clone();
        GetComponent<Tank_Damage>(ComponentType.Damage).ResetData();
    }
}
