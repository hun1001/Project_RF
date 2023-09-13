using Pool;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SubBattery : BaseSubArmament
{
    [SerializeField]
    private LineRenderer _lineRenderer = null;

    public override SATSO GetSATSO() => _satSO;

    private bool _isAiming = false;

    public override void Aim()
    {
        _lineRenderer.enabled = true;
    }

    protected override void OnFire()
    {
        _lineRenderer.enabled = false;

        PoolManager.Get<Shell>("Bullet", FirePoint.position, FirePoint.rotation).SetShell(GetComponent<Tank>(), 200, 40);
        PoolManager.Get("MuzzleFlash4", FirePoint.position, FirePoint.rotation);
    }

    private void Update()
    {
        if(_isAiming)
        {
            _lineRenderer.SetPosition(0, FirePoint.position);
            _lineRenderer.SetPosition(1, FirePoint.position + Vector3.up * 50f);
        }
    }
}
