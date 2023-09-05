using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shell))]
public abstract class Shell_Component : CustomComponent
{
    private Shell _shell = null;

    protected Shell Shell => _shell ??= Instance as Shell;
}
