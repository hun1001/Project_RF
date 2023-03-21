using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : CustomObject
{
    [SerializeField]
    private ShellSO _shellSO = null;
    public ShellSO ShellSO => _shellSO;
}
