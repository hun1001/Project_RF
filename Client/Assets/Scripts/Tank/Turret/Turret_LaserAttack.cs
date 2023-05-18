using Pool;

public class Turret_LaserAttack : Turret_Attack
{
    public override void Fire()
    {
        if (ReloadingTime <= 0 && gameObject.activeSelf)
        {
            PoolManager.Get("LaserBeam", Turret.FirePoint.position, Turret.FirePoint.rotation).GetComponent<LaserBeam>().SetLaserBeam(GetComponent<Tank>(), Turret.FirePoint.position, Turret.FirePoint.position + Turret.FirePoint.up * 500f);
            PoolManager.Get("LaserBeam", (Turret as BossTurret).FirePoint2.position, Turret.FirePoint.rotation).GetComponent<LaserBeam>().SetLaserBeam(GetComponent<Tank>(), (Turret as BossTurret).FirePoint2.position, (Turret as BossTurret).FirePoint2.position + Turret.FirePoint.up * 500f);
            ResetReloadTime();
        }
    }
}
