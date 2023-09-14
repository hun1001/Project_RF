using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionArrowGroupHandle : MonoBehaviour
{
    [SerializeField]
    private ArrowTemplateHandle arrowTemplateHandle = null;

    private List<ArrowTemplateHandle> arrowHandles = new List<ArrowTemplateHandle>();
    private List<ArrowTemplateHandle> unArrowHandles = new List<ArrowTemplateHandle>();


}
