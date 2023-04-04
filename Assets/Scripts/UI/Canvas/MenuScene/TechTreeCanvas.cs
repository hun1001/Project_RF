using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TechTreeCanvas : BaseCanvas
{
    [SerializeField]
    private TechTree _techTree = null;

    [SerializeField]
    private ToggleGroupManager _toggleGroupManager = null;
}
