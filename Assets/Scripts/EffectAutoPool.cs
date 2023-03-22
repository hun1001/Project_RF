using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class EffectAutoPool : AutoPool
{
    [SerializeField]
    private ParticleSystem _effect = null;

    protected override IEnumerator Pool()
    {
        yield return new WaitForSeconds(_effect.main.duration);
        string id = gameObject.name;

        if (id.Contains("(Clone)"))
        {
            id = id.Replace("(Clone)", "");
        }

        PoolManager.Pool(id, gameObject);
    }
}
