using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Missile : MonoBehaviour
{
    private NavMeshPath _navMeshPath = null;
    private TrailRenderer _trailRenderer = null;
    private Vector3 _targetPosition = Vector3.zero;
    private CustomObject _owner = null;
    private float _duration = 1f;

    private void Awake()
    {
        _navMeshPath = new NavMeshPath();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    public void SetMissile(CustomObject owner, Vector3 targetPosition)
    {
        _navMeshPath.ClearCorners();
        _trailRenderer.Clear();

        _owner = owner;

        if (NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, _navMeshPath))
        {
            _targetPosition = _navMeshPath.corners[_navMeshPath.corners.Length - 1];
        }
        else
        {
            _targetPosition = targetPosition;
        }

        transform.DOPath(_navMeshPath.corners, _duration, PathType.CatmullRom).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            Debug.Log("도착!");
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<CustomObject>() != _owner)
        {
            Debug.Log("플레이어에게 데미지!");
        }
    }
}
