using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pool;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _trailRenderer = null;

    private CustomObject _owner = null;
    private float _range = 1.75f;
    private float _duration = 1f;

    private CameraManager _cameraManager = null;

    public void SetMissile(CustomObject owner, Vector3 targetPosition, float range = 1.75f, float duration = 1f)
    {
        _trailRenderer.Clear();

        _owner = owner;
        _range = range;
        _duration = duration;

        _cameraManager ??= Camera.main.GetComponent<CameraManager>();

        transform.DOPath(new[] { transform.position, ((transform.position + targetPosition) / 2) + Vector3.back * 5, targetPosition }, _duration, PathType.Linear)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                var aroundTank = Physics2D.OverlapCircleAll(targetPosition, _range, 1 << LayerMask.NameToLayer("Tank"));

                if (CheckMissileInScreen())
                {
                    _cameraManager.CameraShake(3f, 5f, 0.5f);
                }

                foreach (var tank in aroundTank)
                {
                    if (tank.gameObject != _owner.gameObject)
                    {
                        tank.GetComponent<Tank_Damage>()?.Damaged(50, 99999, targetPosition, _owner.transform.position);
                    }
                }
                PoolManager.Get("MissileExplosionEffect", targetPosition, Quaternion.identity);
                PoolManager.Pool("Missile", gameObject);
            });
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    private bool CheckMissileInScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
