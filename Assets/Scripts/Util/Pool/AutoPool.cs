using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class AutoPool : CustomComponent
{
    private void OnEnable()
    {
        StartCoroutine(nameof(Pool));
    }

    private IEnumerator Pool()
    {
        yield return new WaitForSeconds(2f);
        PoolManager.Pool(Instance.ID, gameObject);
    }
}
