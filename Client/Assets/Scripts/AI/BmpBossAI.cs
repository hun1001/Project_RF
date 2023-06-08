using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BmpBossAI : MonoBehaviour
{
    private Tank _tank = null;
    private NavMeshPath _navMeshPath = null;
    private BehaviorTree _behaviorTree = null;

    private Tank_Move _tankMove = null;
    private Tank_Rotate _tankRotate = null;
    private Tank_Damage _tankDamage = null;


}
