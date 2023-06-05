using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossUgradeAI : MonoBehaviour
{
    private Tank _tank = null;
    private NavMeshPath _navMeshPath = null;
    private BehaviorTree _behaviorTree = null;

    private Tank_Move _tankMove = null;
    private Tank_Rotate _tankRotate = null;
    private Tank_Damage _tankDamage = null;

    private Turret_Rotate _turretRotate = null;
    private BossTurret_Attack _turretAttack = null;
    private Turret_AimLine _turretAimLine = null;

    private Tank _target = null;

    private Vector3 _moveTargetPosition = Vector3.zero;

    
}
