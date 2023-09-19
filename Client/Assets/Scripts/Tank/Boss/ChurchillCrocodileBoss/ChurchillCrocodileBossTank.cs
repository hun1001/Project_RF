using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class ChurchillCrocodileBossTank : BossTank
{
    public override string AI_Address => "ChurchillCrocodileBoss_AI";

    private ChurchillCrocodileBossWeakness _weakness = null;

    private void Start()
    {
        _weakness = PoolManager.Get<ChurchillCrocodileBossWeakness>("Crocodile_Weakness", transform);
        _weakness.transform.localPosition = new Vector3(0, 0, -1.568f);
        _weakness.transform.SetParent(null);
        _weakness.Setting(GetComponent<Rigidbody2D>());
    }
}
