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

    private string _shellExplosionEffectAddress = string.Empty;

    private void Awake()
    {
        (Instance as Shell).TryGetComponent(out _shellSound);
        _shellExplosionEffectAddress = (Instance as Shell).ShellExplosionEffectAddress;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tank"))
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

            if (angle < 90 && angle >= (Instance as Shell).ShellSO.RicochetAngle)
            {
                reflectionDir = Vector2.Reflect(-incidentVector, normalVector);
                transform.up = reflectionDir;
                PoolManager.Get("Ricochet_Old", transform.position, transform.rotation);

                PopupText text = PoolManager.Get<PopupText>("PopupDamage", transform.position + Vector3.back * 5, Quaternion.identity);
                text.SetText("<color=#55ff00><size=1.2>MISS</size></color>");
                text.DoMoveText();

                _shellSound.PlaySound(SoundType.Ricochet, AudioMixerType.Sfx);
            }
            else
            {
                collision.gameObject.GetComponent<Tank_Damage>()?.Damaged((Instance as Shell).Damage, (Instance as Shell).Penetration, collision.contacts[0].point, transform.up);
                PoolManager.Get("TankExplosion_01", transform.position, transform.rotation);
                PoolManager.Pool(Instance.ID, gameObject);
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            PoolManager.Get(_shellExplosionEffectAddress, transform.position, transform.rotation);
            PoolManager.Pool(Instance.ID, gameObject);        
        }
    }
}
