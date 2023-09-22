using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pool;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _trailRenderer = null;

    [SerializeField]
    private AudioClip _fireSound = null;

    [SerializeField]
    private AudioClip _explosionSound = null;

    private CustomObject _owner = null;
    private float _range = 1.75f;
    private float _duration = 1f;

    private CameraManager _cameraManager = null;

    public void SetMissile(CustomObject owner, Vector3 targetPosition, float range = 1.75f, float duration = 1f)
    {
        _trailRenderer.Clear();

        var audioSource = PoolManager.Get<AudioSourceController>("AudioSource", transform.position, Quaternion.identity);
        audioSource.SetSound(_fireSound);
        audioSource.SetGroup(AudioMixerType.Sfx);

        audioSource.Play();

        _owner = owner;
        _range = range;
        _duration = duration;
        
        PoolManager.Get<MissileTargetDisplay>("MissileTargetDisplay").SetTarget(targetPosition, _duration);

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
                        tank.GetComponent<Tank_Damage>()?.Damaged(50, 99999, targetPosition, _owner.transform.position - tank.transform.position);
                    }
                }
                PoolManager.Get("MissileExplosionEffect", targetPosition, Quaternion.identity);

                var audioSource2 = PoolManager.Get<AudioSourceController>("AudioSource", transform.position, Quaternion.identity);
                audioSource2.SetSound(_explosionSound);
                audioSource2.SetGroup(AudioMixerType.Sfx);

                audioSource2.Play();

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
