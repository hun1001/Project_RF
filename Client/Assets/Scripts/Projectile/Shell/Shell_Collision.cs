using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Shell_Collision : Shell_Component
{
    private Tank pt;
    private Tank et;
    private Vector2 normalVector;
    private Vector2 incidentVector;
    private Vector2 reflectionDir;
    private int angle;

    private Shell_Sound _shellSound;
    private void Awake()
    {
        (Instance as Shell).TryGetComponent(out _shellSound);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((Instance as Shell).Owner == collision.gameObject.GetComponent<CustomObject>())
        {
            return;
        }

        pt = (Instance as Shell).Owner as Tank;
        et = collision.gameObject.GetComponent<Tank>();

        if (pt != null && et != null)
        {
            if (pt.GroupType == et.GroupType)
            {
                return;
            }
        }

        normalVector = collision.contacts[0].normal;
        incidentVector = -transform.up;

        angle = (int)Vector2.Angle(incidentVector, normalVector);
        angle %= 180;

        if (angle < 90 && angle >= 60)
        {
            reflectionDir = Vector2.Reflect(-incidentVector, normalVector);

            transform.up = reflectionDir;
            _shellSound.PlaySound(SoundType.Ricochet, AudioMixerType.Sfx);
        }
        else
        {
            collision.gameObject.GetComponent<Tank_Damage>()?.Damaged((Instance as Shell).Damage, (Instance as Shell).Penetration, collision.contacts[0].point);
            PoolManager.Get("Explosion_APHE_01", transform.position, transform.rotation);
            PoolManager.Pool(Instance.ID, gameObject);
        }
    }
}
