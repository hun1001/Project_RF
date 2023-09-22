using Pool;
using System.Collections;
using UnityEngine;

public class EffectWaitPool : EffectAutoPool, IPoolReset
{
    [SerializeField]
    private float _waitTime = 3f;

    public void PoolObjectReset()
    {
       _effectList.ForEach(x => x.Stop());
    }

    protected override IEnumerator Pool()
    {
        _effectList.ForEach(x => x.Play());

        yield return new WaitForSeconds(_waitTime);

        string id = gameObject.name;

        if (id.Contains("(Clone)"))
        {
            id = id.Replace("(Clone)", "");
        }

        PoolManager.Pool(id, gameObject);
    }
}
