using Pool;

public class BossTurret_Attack : Turret_Attack
{
    private Tank _tank = null;
    private BossTurret _bossTurret = null;

    protected override void Awake()
    {
        _tank = GetComponent<Tank>();
        _bossTurret = Turret as BossTurret;
    }

    public override void Fire()
    {
        if (ReloadingTime <= 0 && gameObject.activeSelf)
        {
            PoolManager.Get<LaserBeam>("LaserBeam", Turret.FirePoint.position, Turret.FirePoint.rotation).SetLaserBeam(_tank, Turret.FirePoint, 500f);
            PoolManager.Get<LaserBeam>("LaserBeam", _bossTurret.FirePoint2.position, _bossTurret.FirePoint2.rotation).SetLaserBeam(_tank, _bossTurret.FirePoint2, 500f);
            ResetReloadTime();
        }
    }
}
