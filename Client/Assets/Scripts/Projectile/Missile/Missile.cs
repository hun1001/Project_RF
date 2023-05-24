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

    public void SetMissile(CustomObject owner, Vector3 targetPosition, float range = 1.75f, float duration = 1f)
    {
        _trailRenderer.Clear();

        _owner = owner;
        _range = range;
        _duration = duration;

        transform.DOPath(new[] { transform.position, ((transform.position + targetPosition) / 2) + Vector3.back * 5, targetPosition }, _duration, PathType.Linear)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                var aroundTank = Physics2D.OverlapCircleAll(targetPosition, _range, 1 << LayerMask.NameToLayer("Tank"));
                foreach (var tank in aroundTank)
                {
                    if (tank.gameObject != _owner.gameObject)
                    {
                        tank.GetComponent<Tank_Damage>()?.Damaged(100, 99999, targetPosition, Vector2.zero);
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
}
