using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Shell_Collision : Shell_Component
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Instance as Shell).Owner == collision.GetComponent<CustomObject>())
        {
            return;
        }

        var pt = ((Instance as Shell).Owner as Tank);
        var et = collision.GetComponent<Tank>();

        if (pt != null && et != null)
        {
            if (pt.GroupType == et.GroupType)
            {
                return;
            }
        }

        Vector2 thisCenter = transform.position;
        Vector2 otherCenter = collision.transform.position;

        // 충돌 지점의 벡터를 계산합니다.
        Vector2 collisionVector = collision.ClosestPoint(thisCenter) - thisCenter;

        // 두 벡터 간의 각도를 계산합니다.
        float angle = Vector2.Angle(collisionVector, Vector2.right);

        // 각도를 라디안으로 변환합니다.
        float radians = angle * Mathf.Deg2Rad;

        // 입사각을 구합니다.
        float incidentAngle = Mathf.PI - radians;

        Debug.Log("입사각: " + incidentAngle);

        collision.GetComponent<Tank_Damage>().Damaged((Instance as Shell).Damage, (Instance as Shell).Penetration);
        PoolManager.Get("Explosion_APHE_01", transform.position, transform.rotation);
        PoolManager.Pool(Instance.ID, gameObject);
    }

    private float GetAngleOfIncidence(Vector2 normal)
    {
        var angle = Vector2.Angle(normal, Vector3.up);
        return angle;
    }
}
