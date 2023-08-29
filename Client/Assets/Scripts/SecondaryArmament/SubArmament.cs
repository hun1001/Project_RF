using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubArmament : MonoBehaviour
{
    public abstract SubArmamentKeyActionType ActionType { get; }
    public abstract void Fire();
}
