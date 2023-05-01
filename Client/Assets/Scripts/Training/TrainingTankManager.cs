using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Addressable;
using System.Linq;

public class TrainingTankManager : MonoBehaviour
{
    [SerializeField]
    private Transform _tankTransform = null;

    [SerializeField]
    private Dropdown _tankDropdown = null;

    private List<GameObject> _tankList = new List<GameObject>();

    private string groupName = "Tank";

    private void Awake()
    {

    }
}
