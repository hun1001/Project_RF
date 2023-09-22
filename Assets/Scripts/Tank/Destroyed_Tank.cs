using System.Collections;
using Pool;
using UnityEngine;

public class Destroyed_Tank : MonoBehaviour
{
    private IEnumerator PoolCoroutine()
    {
        yield return new WaitForSeconds(3f);
        PoolManager.Pool("Destroyed_Tank", this.gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(PoolCoroutine());
    }
}
