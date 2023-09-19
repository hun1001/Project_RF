using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchillCrocodileBossWeakness : MonoBehaviour
{
    [SerializeField]
    private DistanceJoint2D _distanceJoint2D = null;

    public void Setting(Rigidbody2D rigidbody2D)
    {
        _distanceJoint2D.connectedBody = rigidbody2D;
    }
}
