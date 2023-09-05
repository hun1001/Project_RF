using Pool;
using System.Collections;
using UnityEngine;

public class EffectWaitPool : AutoPool
{
    [SerializeField]
    private float _waitTime = 3f;

    protected override IEnumerator Pool()
    {
        yield return new WaitForSeconds(_waitTime);

        string id = gameObject.name;

        if (id.Contains("(Clone)"))
        {
            id = id.Replace("(Clone)", "");
        }

        PoolManager.Pool(id, gameObject);
    }
}
