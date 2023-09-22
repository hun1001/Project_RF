using Addressable;
using UnityEngine;
using UnityEngine.UI;

public class MenuTankInfoUI : MonoBehaviour
{
    [SerializeField]
    private Image _tankCountryImage = null;
    [SerializeField]
    private Image _tankTypeImage = null;
    [SerializeField]
    private TextController _tankTierText = null;
    [SerializeField]
    private TextController _tankNameText = null;

    [SerializeField]
    private Sprite[] _countrySprites = null;

    public void CurrentTankInfoUpdate()
    {
        string currentTankID = PlayerDataManager.Instance.GetPlayerTankID();
        Tank tank = AddressablesManager.Instance.GetResource<GameObject>(currentTankID).GetComponent<Tank>();
        var techTree = FindObjectOfType<TechTreeCanvas>();

        _tankCountryImage.sprite = _countrySprites[(int)tank.TankSO.CountryType - 1];
        _tankTypeImage.sprite = techTree.GetTankTypeSprite(tank.TankSO.TankType);
        _tankTierText.SetText(techTree.TankTierNumber[tank.TankSO.TankTier - 1]);
        _tankNameText.SetText(currentTankID);
    }
}
