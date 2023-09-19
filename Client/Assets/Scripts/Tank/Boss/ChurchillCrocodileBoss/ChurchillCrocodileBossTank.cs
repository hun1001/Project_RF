using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchillCrocodileBossTank : BossTank
{
    public override string AI_Address => "ChurchillCrocodileBoss_AI";

    [SerializeField]
    private GameObject _weakness = null;

    private void Start()
    {
        _weakness = Instantiate(_weakness, new Vector3(0, -4.16f, -1.568f), Quaternion.identity);
    }
}
