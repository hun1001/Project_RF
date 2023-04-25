using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Rotate : Tank_Component
{
    private Vector3 _direction = Vector3.zero;
    private float _rotationSpeed => (Instance as Tank).TankData.RotationSpeed;

    public void Rotate(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            direction.Normalize();

            _direction.x = -direction.x;
            _direction.y = 0;
            _direction.z = direction.y;

            Quaternion targetRotation = Quaternion.LookRotation(_direction);

            // 바로 봄
            //transform.rotation = Quaternion.Euler(0, 0, targetRotation.eulerAngles.y);

            // 기존 원하는 위치로 일정하게 회전하는 코드 그러나 180회전할 때 속도가 90도 회전보다 더 빠름
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetRotation.eulerAngles.y), Time.deltaTime / (1f / (_rotationSpeed / 360f)));

            // 초당 일정하게 회전하는 코드
            float maxRotationDelta = _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetRotation.eulerAngles.y), maxRotationDelta);
        }
    }
}
