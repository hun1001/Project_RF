using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : CustomObject
{
    [SerializeField]
    private BulletSO _bulletSO = null;
    public BulletSO BulletSO => _bulletSO;
}
