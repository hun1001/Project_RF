using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public abstract class Tank_Component : CustomComponent
{
    protected virtual void Update()
    {
        if((Instance as Tank).IsDead)
        {
            return;
        }
    }
}
