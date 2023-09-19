using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class ChurchillCrocodileBossTurret_Attack : Turret_Attack
{
    private Flame flame = null;

    private void Start()
    {
        flame = PoolManager.Get<Flame>("Flame", transform);
        flame.SetActive(false);
    }

    public void Flamethrow()
    {
        StartCoroutine(FlamethrowCoroutine());
    }

    private IEnumerator FlamethrowCoroutine()
    {
        flame.SetActive(true);
        yield return new WaitForSeconds(3f);
        flame.SetActive(false);
    }
}
