using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using DG.Tweening;

public class LaserBeam : CustomObject
{
    [SerializeField]
    private LineRenderer _lineRenderer = null;

    [SerializeField]
    private MeshRenderer _meshRenderer = null;

    [SerializeField]
    private BoxCollider2D _collider = null;

    [SerializeField]
    private AudioSource _audioSource = null;

    private CustomObject _owner = null;
    private Transform _fireTransform = null;
    private float _length = 0;

    private float _chargeTime = 0;
    private float _duration = 0;

    private CameraManager _cameraManager = null;

    public void SetLaserBeam(CustomObject owner, Transform fireTransform, float length, float chargeTime = 1f, float duration = 1f)
    {
        _owner = owner;

        _meshRenderer.enabled = false;
        _collider.enabled = false;

        _cameraManager ??= Camera.main.GetComponent<CameraManager>();

        _length = length;

        _fireTransform = fireTransform;

        _chargeTime = chargeTime;
        _duration = duration;

        StartCoroutine(LaserBeamCoroutine());
    }

    private IEnumerator LaserBeamCoroutine()
    {
        _audioSource.Play();
        StartCoroutine(nameof(LaserLineUpdateCoroutine));
        yield return new WaitForSeconds(_chargeTime);

        StopCoroutine(nameof(LaserLineUpdateCoroutine));

        transform.localScale = new Vector3(1, _length / 2, 1);
        transform.position = (_fireTransform.position + _fireTransform.position + _fireTransform.up * _length) / 2;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_fireTransform.up.y, _fireTransform.up.x) * Mathf.Rad2Deg + 90);

        _meshRenderer.enabled = true;
        _collider.enabled = true;

        _cameraManager.CameraShake(3, 5, 0.5f);

        yield return new WaitForSeconds(_duration);

        _collider.enabled = false;

        transform.DOScaleX(0, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => PoolManager.Pool(ID, gameObject));
    }

    private IEnumerator LaserLineUpdateCoroutine()
    {
        while (true)
        {
            SetLaserTransform();
            yield return null;
        }
    }

    private void SetLaserTransform()
    {
        _lineRenderer.SetPosition(0, _fireTransform.position);
        _lineRenderer.SetPosition(1, _fireTransform.position + _fireTransform.up * _length);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<CustomObject>() != _owner)
        {
            other.GetComponent<Tank_Damage>().Damaged(1, 99999, other.ClosestPoint(transform.position), Vector2.zero);
        }
    }
}
