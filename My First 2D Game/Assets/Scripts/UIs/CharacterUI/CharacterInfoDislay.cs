using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoDislay : MonoBehaviour
{
    [SerializeField] InfoDisplay CharacterDescription;
    [SerializeField] Text MainAttribute;
    [SerializeField] Text SubAttribute1;
    [SerializeField] Text SubAttribute2;

    public Image EquipmentIcon;
    public string EquipmentTag { get; set; }
    public int EquipmentNum { get; set; }
    public List<int[]> EquipmentSpecials { get; set; }

    public void EquipmentHighlighted()
    {
        UIController.Instance.ToggleCharacterDescription(true);
        switch (EquipmentTag)
        {
            case "Weapon":
                CharacterDescription.DisplayWeaponInfo(EquipmentNum, EquipmentSpecials, false);
                break;

            case "Shield":
                CharacterDescription.DisplayShieldInfo(EquipmentNum, EquipmentSpecials, false);
                break;

            case "Amulet":
                CharacterDescription.DisplayJewelryInfo(EquipmentTag, EquipmentNum, EquipmentSpecials, false);
                break;

            case "Ring":
                CharacterDescription.DisplayJewelryInfo(EquipmentTag, EquipmentNum, EquipmentSpecials, false);
                break;
        }
    }

    public void StatsHighlighted(string StatsName)
    {
        UIController.Instance.ToggleCharacterDescription(true);
        switch (StatsName)
        {
            case "Strength":
                CharacterDescription.DisplayStrengthInfo();
                break;

            case "Agility":
                CharacterDescription.DisplayAgilityInfo();
                break;

            case "Intelligence":
                CharacterDescription.DisplayIntelligenceInfo();
                break;
        }
    }

    public void Dehighlighted()
    {
        UIController.Instance.ToggleCharacterDescription(false);
    }

    public void UpdateStatsValue(int Main, int Sub1, int Sub2)
    {
        MainAttribute.text = Main.ToString();
        SubAttribute1.text = Sub1.ToString();
        SubAttribute2.text = Sub2.ToString();
    }
}
