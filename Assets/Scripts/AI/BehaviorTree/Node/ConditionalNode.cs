using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : DecoratorNode
{
    public override bool Execute()
    {
        return true;
    }
}
