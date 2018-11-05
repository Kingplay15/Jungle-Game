using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] Text Name;
    [SerializeField] Text Desc;
    [SerializeField] Text SpecialBonuses;

    Item QuestItemToDisplay;
    ConsumableItem ConsumableToDisplay;
    Weapon WeaponToDisplay;
    Shield ShieldToDisplay;
    Jewelry JewelryToDisplay;
    string SpecialsToDisplay = "";
    string RarityToDisplay = "";
    string FinalRequirementDisplay = "";

    public void DisplayConsumableInfo(int ItemNum)
    {
        ItemDatabase.Instance.GetConsumables.TryGetValue(ItemNum, out ConsumableToDisplay);

        Name.color = Color.black;
        Name.text = ConsumableToDisplay.Name;
        Desc.text = "Usage: " + ConsumableToDisplay.Description;
        SpecialBonuses.text = "";
    }

    public void DisplayWeaponInfo(int ItemNum, List<int[]> Specials, bool UnableEquipping)
    {
        ItemDatabase.Instance.GetWeapons.TryGetValue(ItemNum, out WeaponToDisplay);
        GeneralDisplay(Specials);

        //Formatting Requirement text to display
        string[] RequirementDisplay = new string[]
            {
            "Level " + WeaponToDisplay.Requirement[0].ToString()+ " ",
            "Strength " + WeaponToDisplay.Requirement[1].ToString()+ " ",
            "Agility " + WeaponToDisplay.Requirement[2].ToString()+ " ",
            "Intelligence " + WeaponToDisplay.Requirement[3].ToString()
            };

        for (int i = 0; i < 4; i++)
        {
            if (WeaponToDisplay.Requirement[i] == 0)
                RequirementDisplay[i] = "";
            FinalRequirementDisplay += RequirementDisplay[i];
        }

        if (FinalRequirementDisplay != "")
        {
            if (UnableEquipping)
                FinalRequirementDisplay = "<color=red>" + "<b>Requirement: </b>" + FinalRequirementDisplay + "</color>";
            else FinalRequirementDisplay = "<color=black>" + "<b>Requirement: </b>" + FinalRequirementDisplay + "</color>";
        }

        Name.text = RarityToDisplay + WeaponToDisplay.Name;
        if (FinalRequirementDisplay == "")
            Desc.text = "<i>" + WeaponToDisplay.Description + "</i>" + Environment.NewLine +
            "<b>Damage: </b>" + WeaponToDisplay.Damage + Environment.NewLine +
            "<b>Speed: </b>" + WeaponToDisplay.Speed.ToString();
        else Desc.text = "<i>" + WeaponToDisplay.Description + "</i>" + Environment.NewLine +
            "<b>Damage: </b>" + WeaponToDisplay.Damage + Environment.NewLine +
            "<b>Speed: </b>" + WeaponToDisplay.Speed.ToString() + Environment.NewLine + FinalRequirementDisplay;
        SpecialBonuses.text = SpecialsToDisplay;
    }

    public void DisplayShieldInfo(int ItemNum, List<int[]> Specials, bool UnableEquipping)
    {
        ItemDatabase.Instance.GetShields.TryGetValue(ItemNum, out ShieldToDisplay);
        GeneralDisplay(Specials);

        //Formatting Requirement text to display
        string[] RequirementDisplay = new string[]
            {
            "Level " + ShieldToDisplay.Requirement[0].ToString()+ " ",
            "Strength " + ShieldToDisplay.Requirement[1].ToString()+ " ",
            "Agility " + ShieldToDisplay.Requirement[2].ToString()+ " ",
            "Intelligence " + ShieldToDisplay.Requirement[3].ToString()
            };

        for (int i = 0; i < 4; i++)
        {
            if (ShieldToDisplay.Requirement[i] == 0)
                RequirementDisplay[i] = "";
            FinalRequirementDisplay += RequirementDisplay[i];
        }

        if (FinalRequirementDisplay != "")
        {
            if (UnableEquipping)
                FinalRequirementDisplay = "<color=red>" + "<b>Requirement: </b>" + FinalRequirementDisplay + "</color>";
            else FinalRequirementDisplay = "<color=black>" + "<b>Requirement: </b>" + FinalRequirementDisplay + "</color>";
        }

        Name.text = RarityToDisplay + ShieldToDisplay.Name;
        if (FinalRequirementDisplay == "")
            Desc.text = "<i>" + ShieldToDisplay.Description + "</i>" + Environment.NewLine +
                "<b>Defense: </b>" + ShieldToDisplay.Defense;
        else Desc.text = "<i>" + ShieldToDisplay.Description + "</i>" + Environment.NewLine +
            "<b>Defense: </b>" + ShieldToDisplay.Defense + Environment.NewLine + FinalRequirementDisplay;
        SpecialBonuses.text = SpecialsToDisplay;
    }

    public void DisplayJewelryInfo(string ItemTag, int ItemNum, List<int[]> Specials, bool UnableEquipping)
    {
        if (ItemTag == "Amulet")
            ItemDatabase.Instance.GetAmulets.TryGetValue(ItemNum, out JewelryToDisplay);
        else ItemDatabase.Instance.GetRings.TryGetValue(ItemNum, out JewelryToDisplay);
        GeneralDisplay(Specials);

        //Formatting Requirement text to display
        string[] RequirementDisplay = new string[]
            {
            "Level " + JewelryToDisplay.Requirement[0].ToString()+ " ",
            "Strength " + JewelryToDisplay.Requirement[1].ToString()+ " ",
            "Agility " + JewelryToDisplay.Requirement[2].ToString()+ " ",
            "Intelligence " + JewelryToDisplay.Requirement[3].ToString()
            };

        for (int i = 0; i < 4; i++)
        {
            if (JewelryToDisplay.Requirement[i] == 0)
                RequirementDisplay[i] = "";
            FinalRequirementDisplay += RequirementDisplay[i];
        }

        if (FinalRequirementDisplay != "")
        {
            if (UnableEquipping)
                FinalRequirementDisplay = "<color=red>" + "<b>Requirement: </b>" + FinalRequirementDisplay + "</color>";
            else FinalRequirementDisplay = "<color=black>" + "<b>Requirement: </b>" + FinalRequirementDisplay + "</color>";
        }

        Name.text = RarityToDisplay + JewelryToDisplay.Name;
        if (FinalRequirementDisplay == "")
            Desc.text = "<i>" + JewelryToDisplay.Description + "</i>";
        else Desc.text = "<i>" + JewelryToDisplay.Description + "</i>" + Environment.NewLine + FinalRequirementDisplay;
        SpecialBonuses.text = SpecialsToDisplay;
    }

    public void DisplayQuestItemInfo(int ItemNum)
    {
        ItemDatabase.Instance.GetQuests.TryGetValue(ItemNum, out QuestItemToDisplay);

        Name.color = Color.black;
        Name.text = QuestItemToDisplay.Name;
        Desc.text = QuestItemToDisplay.Description;
        SpecialBonuses.text = "";
    }

    void GeneralDisplay(List<int[]> Specials)
    {
        SpecialsToDisplay = "";
        RarityToDisplay = "";
        Name.color = Color.black;
        FinalRequirementDisplay = "";
        SpecialBonus tempDesc;
        if (Specials.Count == 0)
            return;
        switch (Specials.Count)
        {
            case 1:
                RarityToDisplay = "Rare" + Environment.NewLine;
                Name.color = Color.blue;
                break;
            case 2:
                RarityToDisplay = "Ultra Rare" + Environment.NewLine;
                Name.color = Color.magenta;
                break;
            case 3:
                RarityToDisplay = "Legendary" + Environment.NewLine;
                Name.color = Color.red;
                break;
            case 5:
                Name.color = new Color32(80, 0, 217, 255);
                break;
        }

        for (int i = 0; i < Specials.Count; i++)
        {
            ItemDatabase.Instance.GetSpecialBonuses.TryGetValue(Specials[i][0], out tempDesc);
            if (Specials[i][1] == 0)
                SpecialsToDisplay += tempDesc.Description + Environment.NewLine;
            else SpecialsToDisplay += tempDesc.Description + Specials[i][1] + Environment.NewLine;
        }
    }

    public void DisplayStrengthInfo()
    {
        Name.color = Color.red;
        Name.text = "STRENGTH";
        Desc.text = "Every point increases attack damage by " +
            PlayerStats.DamageModifier + "." + Environment.NewLine +
            "Every point increases max health by " + PlayerStats.HealthModifier + ".";

        SpecialBonuses.text = "";
    }

    public void DisplayAgilityInfo()
    {
        Name.color = Color.green;
        Name.text = "AGILITY";
        Desc.text = "Every point increases defense by " + PlayerStats.DefenseModifier + "." + Environment.NewLine +
            "Every point increases attack speed by " + PlayerStats.AttackSpeedModifier + ".";

        SpecialBonuses.text = "";
    }

    public void DisplayIntelligenceInfo()
    {
        Name.color = Color.blue;
        Name.text = "INTELLIGENCE";
        Desc.text = "Every point increases spell power by " +
            PlayerStats.SpellPowerModifier + "." + Environment.NewLine +
            "Every point increases mana by " + PlayerStats.ManaModifier + ".";

        SpecialBonuses.text = "";
    }

    public void DisplayClassInfo(string ClassName)
    {
        switch (ClassName)
        {
            case "Knight":
                Name.color = Color.red;
                Name.text = "KNIGHT";
                Desc.text = "A former royal head-guardian, he's powerful with his melee combat skills.";
                SpecialBonuses.text = "";
                break;
            case "Archer":
                Name.color = Color.green;
                Name.text = "ARCHER";
                Desc.text = "Famous Lone Ranger of the WitchWood, hunting down monsters by skillful archery is his ability.";
                SpecialBonuses.text = "";
                break;
            case "Mage":
                Name.color = Color.blue;
                Name.text = "MAGE";
                Desc.text = "The great wizard of the Arcarne Tower, he has mastered elmental magics and use them to destroy enemies.";
                SpecialBonuses.text = "";
                break;
        }
    }

    public void DisplaySaveSlotInfo()
    {
        Name.color = Color.red;
        Name.text = SaveLoadController.Instance.MainPlayerData.Name;
        Desc.color = Color.blue;
        Desc.text = "CLASS: " + SaveLoadController.Instance.MainPlayerData.Class;
        SpecialBonuses.color = Color.green;
        SpecialBonuses.text = "LEVEL: " + SaveLoadController.Instance.MainPlayerData.Level.ToString();
    }
}
