using System.Collections;
using UnityEngine;
using Pool;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    private Tank _tank = null;
    private NavMeshPath _navMeshPath = null;

    private void Start()
    {
        _tank = PoolManager.Get("BMP-130-2").GetComponent<Tank>();
        _navMeshPath = new NavMeshPath();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (NavMesh.CalculatePath(_tank.transform.position, hitInfo.point, NavMesh.AllAreas, _navMeshPath))
                {
                    for (int i = 0; i < _navMeshPath.corners.Length - 1; i++)
                    {
                        Debug.DrawLine(_navMeshPath.corners[i], _navMeshPath.corners[i + 1], Color.red, 5f);
                    }
                }
            }
        }
    }

    private IEnumerator MoveTarget()
    {
        while (true)
        {
            yield return null;
        }
    }
}
