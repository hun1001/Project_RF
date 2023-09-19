using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionArrowGroupHandle : MonoBehaviour
{
    [SerializeField]
    private ArrowTemplateHandle arrowTemplateHandle = null;

    private Queue<ArrowTemplateHandle> unArrowHandles = new Queue<ArrowTemplateHandle>();

    private void Start()
    {
        unArrowHandles.Clear();
    }

    public void AddEnemyPositionArrow(Tank tank)
    {
        var arrow = GetArrow();
        
        arrow.SetTargetTank(tank, PoolArrow);
    }

    private ArrowTemplateHandle GetArrow()
    {
        if (unArrowHandles.Count > 0)
        {
            var arrow = unArrowHandles.Dequeue();
            arrow.gameObject.SetActive(true);
            return arrow;
        }
        else
        {
            var arrow = Instantiate(arrowTemplateHandle, transform);
            arrow.gameObject.SetActive(true);
            return arrow;
        }
    }
    
    private void PoolArrow(ArrowTemplateHandle arrow)
    {
        arrow.gameObject.SetActive(false);
        unArrowHandles.Enqueue(arrow);
    }
}
