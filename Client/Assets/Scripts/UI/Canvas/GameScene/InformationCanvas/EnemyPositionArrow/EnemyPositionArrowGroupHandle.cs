using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionArrowGroupHandle : MonoBehaviour
{
    [SerializeField]
    private ArrowTemplateHandle arrowTemplateHandle = null;

    private Queue<ArrowTemplateHandle> arrowHandles = new Queue<ArrowTemplateHandle>();
    private Queue<ArrowTemplateHandle> unArrowHandles = new Queue<ArrowTemplateHandle>();

    public void AddEnemyPositionArrow(Tank tank)
    {
        
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
}
