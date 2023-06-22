using UnityEngine;
using TMPro;

public class TechTreeTierLine : MonoBehaviour
{
    [SerializeField]
    private TechTree _techTree = null;

    [SerializeField]
    private RectTransform _tankTierLine = null;

    [SerializeField]
    private GameObject _firstHorizontalLine = null;

    [SerializeField]
    private GameObject _tankTierTemplate = null;

    [SerializeField]
    private GameObject _tankTierConnectLineTemplate = null;


    public void SetTierLine(int maxLength)
    {
        _firstHorizontalLine.SetActive(maxLength > 1);

        for (int i = 0; i < maxLength; i++)
        {
            var tankTier = Instantiate(_tankTierTemplate, _tankTierLine);
            var tankTierConnectLine = Instantiate(_tankTierConnectLineTemplate, _tankTierLine);

            tankTier.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _techTree.TankTierNumber[i];

            tankTier.SetActive(true);
            tankTierConnectLine.SetActive(true);
        }
    }

    public void ResetTierLine()
    {
        for (int i = 3; i < _tankTierLine.transform.childCount; ++i)
        {
            Destroy(_tankTierLine.transform.GetChild(i).gameObject);
        }
    }
}
