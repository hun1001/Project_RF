using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SATSO", menuName = "ScriptableObjects/SATSO")]
public class SATSO : ScriptableObject
{
    public int BeltCapacity = 0;
    public float ReloadTime = 0f;
}
