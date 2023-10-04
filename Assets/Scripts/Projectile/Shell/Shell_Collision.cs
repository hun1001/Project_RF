using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using Event;

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

    public bool _isBasicCollisionLogic = true;

    private void Awake()
    {
        Shell.TryGetComponent(out _shellSound);
        _shellExplosionEffectAddress = Shell.ShellExplosionEffectAddress;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //var enhance = Shell.Owner.GetComponent<Tank>().Enhancement.GetShellEnhance(Shell.ShellType);

        //for (int i = 0; i < enhance.Length; ++i)
        //{
        //    enhance[i].Collision(Shell);
        //}

        if (_isBasicCollisionLogic == false)
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Tank"))
        {
            if (Shell.Owner == collision.gameObject.GetComponent<CustomObject>())
            {
                return;
            }

            pt = Shell.Owner as Tank;
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

            // Ricochet!
            if (angle < 90 && angle >= (Instance as Shell).ShellSO.RicochetAngle)
            {
                reflectionDir = Vector2.Reflect(-incidentVector, normalVector);
                transform.up = reflectionDir;
                PoolManager.Get("Ricochet_Old", transform.position, transform.rotation);

                PopupText text = PoolManager.Get<PopupText>("PopupDamage", transform.position + Vector3.back * 5, Quaternion.identity);
                text.SetText("<color=#55ff00><size=1.2>MISS</size></color>");
                text.DoMoveText();

                _shellSound.PlaySound(SoundType.Ricochet, AudioMixerType.Sfx);

                if (TutorialManager.Instance.IsTutorial)
                {
                    EventManager.TriggerEvent("Ricochet");
                }
            }
            // Not Ricochet
            else
            {
                collision.gameObject.GetComponent<Tank_Damage>()?.Damaged(Shell.Damage, Shell.Penetration, collision.contacts[0].point, transform.up);
                PoolManager.Get(_shellExplosionEffectAddress, transform.position, transform.rotation);
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
