using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Shell
{
    public class Shell_Pool : Base.CustomComponent<Shell>
    {
        private float _lifeTime = 1f;

        private void OnEnable()
        {
            _lifeTime = Instance.Range / Instance.Speed * 2;
            StartCoroutine(DisableShell());
        }

        private IEnumerator DisableShell()
        {
            yield return new WaitForSeconds(_lifeTime);
            PoolManager.Instance.Pool("Assets/Prefabs/Shell/Shell.prefab", this.gameObject);
        }
    }
}
