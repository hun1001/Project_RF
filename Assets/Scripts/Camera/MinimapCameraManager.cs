using UnityEngine;

public class MinimapCameraManager : MonoBehaviour
{
    /// <summary>  미니맵 카메라  </summary>
    private Camera _cam;
    /// <summary> 카메라 사이즈 </summary>
    private Vector2 _size;
    private float _reciprocal;
    private float _rotation;
    private Vector2 _distance = Vector2.zero;
    /// <summary>  적의 포지션  </summary>
    private Vector3 _targetPosition = Vector3.zero;


    private void Start()
    {
        TryGetComponent(out _cam);
        _size = new Vector2(_cam.orthographicSize - 7.5f, _cam.orthographicSize * _cam.aspect - 7.5f);
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    /// <summary> 화살표의 각도와 위치를 설정하는 함수 </summary>
    /// <param name="target"> 적 </param>
    /// <param name="indicator"> 해당 적을 가르키는 화살표 </param>
    public void IndicatorPositionSetting(Transform target, Transform indicator)
    {
        _targetPosition = target.position;
        _distance = new Vector2(transform.position.x - _targetPosition.x, transform.position.y - _targetPosition.y);

        _distance = Quaternion.Euler(0, 0, target.eulerAngles.y) * _distance;

        // X axis
        if (Mathf.Abs(_distance.x) > Mathf.Abs(_distance.y))
        {
            _reciprocal = Mathf.Abs(_size.x / _distance.x);
        }
        // Y axis
        else
        {
            _reciprocal = Mathf.Abs(_size.y / _distance.y);
        }
        _rotation = Mathf.Atan2(transform.position.y - _targetPosition.y, transform.position.x - _targetPosition.x) * Mathf.Rad2Deg - 90f;

        indicator.localPosition = new Vector3(_distance.x * -_reciprocal, _distance.y * -_reciprocal, 1);
        indicator.localEulerAngles = new Vector3(0, 0, _rotation);
    }
}
