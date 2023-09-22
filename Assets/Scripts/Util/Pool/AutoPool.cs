using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class AutoPool : CustomComponent
    {
        private void OnEnable()
        {
            StartCoroutine(nameof(Pool));
        }

        protected virtual IEnumerator Pool()
        {
            yield return new WaitForSeconds(2f);
            PoolManager.Pool(Instance.ID, gameObject);
        }
    }
}