using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEShellEnhance : ShellEnhance
{
    public override void Explosion(Shell shell)
    {
        var targets = Physics2D.OverlapCircleAll(shell.transform.position, 10f, 1 << LayerMask.NameToLayer("Tank"));

       foreach (var target in targets) { }

        Debug.Log(targets.Length);
    }

    private void OnDrawGizmos()
    {

    }
}
