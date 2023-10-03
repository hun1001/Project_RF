using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEShellEnhance : ShellEnhance
{
    private Shell _shell = null;

    public override void Collision(Shell shell)
    {
        _shell = shell;

        var targets = Physics2D.OverlapCircleAll(shell.transform.position, 10f, 1 << LayerMask.NameToLayer("Tank"));

       foreach (var target in targets) { }

        Debug.Log(targets.Length);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(_shell.transform.position, 10f);
    //}
}
