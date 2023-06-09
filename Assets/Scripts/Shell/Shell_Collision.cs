﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Sound;

namespace Shell
{
    public class Shell_Collision : Base.CustomComponent<Shell>
    {
        private void OnCollisionEnter(Collision other)
        {
            float angle = Vector3Calculator.GetIncomingAngle(Instance.transform.forward.normalized, other.contacts[0].normal);

            if (other.transform.CompareTag("Map") == true)
            {
                switch (TypeReader.Instance.GetHitType(angle))
                {
                    case HitType.PENETRATION:
                        PoolManager.Instance.Get("Assets/Prefabs/Effect/WFX_ExplosiveSmoke.prefab", Instance.transform.position, Quaternion.identity);
                        PoolManager.Instance.Pool(name, gameObject);
                        break;
                    case HitType.RICOCHET:
                        transform.rotation = Quaternion.LookRotation(Vector3Calculator.GetReflectionVector(Instance.transform.forward.normalized, other.contacts[0].normal));
                        SoundManager.Instance.PlaySound(Instance.MapRicochetSound, SoundType.SFX);
                        break;
                }
            }
            else if (other.transform.CompareTag("Shell") == true)
            {

            }
            else
            {
                switch (TypeReader.Instance.GetHitType(angle))
                {
                    case HitType.PENETRATION:
                        PoolManager.Instance.Get("Assets/Prefabs/Effect/WFX_ExplosiveSmoke.prefab", Instance.transform.position, Quaternion.identity);
                        PoolManager.Instance.Pool(name, gameObject);
                        other.gameObject.SendMessage("OnHit", Instance.ShellSO.baseDamage, SendMessageOptions.DontRequireReceiver);
                        break;
                    case HitType.RICOCHET:
                        transform.rotation = Quaternion.LookRotation(Vector3Calculator.GetReflectionVector(Instance.transform.forward.normalized, other.contacts[0].normal));
                        SoundManager.Instance.PlaySound(Instance.RicochetSound, SoundType.SFX);
                        PoolManager.Instance.Get<Effect.TextEffect>("Assets/Prefabs/UI/TextEffect.prefab", transform.position, Quaternion.identity).SetTextEffect("Ricochet", Color.white, 2, 1f);
                        break;
                }
                #region 3번 스킬 테스트
                if (other.gameObject.CompareTag("PlayerTank") && TypeReader.Instance.RicochetAngle != 30f)
                    TypeReader.Instance.RicochetAngle = 30f;
                #endregion
            }
        }
    }
}
