using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer _trailRenderer = null;

    private Vector3 _targetPosition = Vector3.zero;
    private CustomObject _owner = null;
    private float _duration = 1f;

    public void SetMissile(CustomObject owner, Vector3 targetPosition)
    {
        _trailRenderer.Clear();

        _owner = owner;

        Vector3 startPosition = transform.position;

        transform.DOPath(new[] { transform.position, targetPosition }, _duration, PathType.CatmullRom).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            Debug.Log("도착!");
            var aroundTank = Physics2D.OverlapCircleAll(targetPosition, 5f, 1 << LayerMask.NameToLayer("Tank"));

            foreach (var tank in aroundTank)
            {
                if (tank.gameObject != _owner.gameObject)
                {
                    tank.GetComponent<Tank_Damage>()?.Damaged(100, 99999, targetPosition, Vector2.zero);
                }
            }
        });
    }
}
