using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    //Singleton
    public static ItemDatabase Instance = null;

    ItemList MyItemList = new ItemList();
    SpecialBonusList MySpecialBonusList = new SpecialBonusList();

    //Consumables
    Dictionary<int, ConsumableItem> ConsumablesDict = new Dictionary<int, ConsumableItem>(); //For looking up 
    public Dictionary<int, ConsumableItem> GetConsumables
    {
        get { return ConsumablesDict; }
    }

    //Weapons
    Dictionary<int, Weapon> WeaponsDict = new Dictionary<int, Weapon>(); //For looking up 
    public Dictionary<int, Weapon> GetWeapons
    {
        get { return WeaponsDict; }
    }

    //Shields
    Dictionary<int, Shield> ShieldsDict = new Dictionary<int, Shield>(); //For looking up 
    public Dictionary<int, Shield> GetShields
    {
        get { return ShieldsDict; }
    }

    //Amulets
    Dictionary<int, Jewelry> AmuletsDict = new Dictionary<int, Jewelry>(); //For looking up 
    public Dictionary<int, Jewelry> GetAmulets
    {
        get { return AmuletsDict; }
    }

    //Rings
    Dictionary<int, Jewelry> RingsDict = new Dictionary<int, Jewelry>(); //For looking up 
    public Dictionary<int, Jewelry> GetRings
    {
        get { return RingsDict; }
    }

    //Quests
    Dictionary<int, Item> QuestsDict = new Dictionary<int, Item>(); //For looking up 
    public Dictionary<int, Item> GetQuests
    {
        get { return QuestsDict; }
    }

    //SpecialBonuses
    Dictionary<int, SpecialBonus> SpecialBonusesDict = new Dictionary<int, SpecialBonus>(); //For looking up 
    public Dictionary<int, SpecialBonus> GetSpecialBonuses
    {
        get { return SpecialBonusesDict; }
    }

    string ItemListString;
    string SpecialBonusListString;

    void Awake()
    {
        //There has to be only ONE instance of Itemdatabase in the game
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        ItemListString = File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json");
        MyItemList = JsonUtility.FromJson<ItemList>(ItemListString);

        SpecialBonusListString = File.ReadAllText(Application.dataPath + "/StreamingAssets/SpecialBonuses.json");
        MySpecialBonusList = JsonUtility.FromJson<SpecialBonusList>(SpecialBonusListString);

        ConstructConsumables();
        ConstructWeapons();
        ConstructShields();
        ConstructAmulets();
        ConstructRings();
        ConstructQuests();
        ConstructSpecials();
    }

    void ConstructConsumables()
    {
        for (int i = 0; i < MyItemList.Consumables.Count; i++)
            ConsumablesDict.Add(i, MyItemList.Consumables[i]);
    }

    void ConstructWeapons()
    {
        for (int i = 0; i < MyItemList.Weapons.Count; i++)
            WeaponsDict.Add(i, MyItemList.Weapons[i]);
    }

    void ConstructShields()
    {
        for (int i = 0; i < MyItemList.Shields.Count; i++)
            ShieldsDict.Add(i, MyItemList.Shields[i]);
    }

    void ConstructAmulets()
    {
        for (int i = 0; i < MyItemList.Amulets.Count; i++)
            AmuletsDict.Add(i, MyItemList.Amulets[i]);
    }

    void ConstructRings()
    {
        for (int i = 0; i < MyItemList.Rings.Count; i++)
            RingsDict.Add(i, MyItemList.Rings[i]);
    }

    void ConstructQuests()
    {
        for (int i = 0; i < MyItemList.Quests.Count; i++)
            QuestsDict.Add(i, MyItemList.Quests[i]);
    }

    void ConstructSpecials()
    {
        for (int i = 0; i < MySpecialBonusList.Specials.Count; i++)
            SpecialBonusesDict.Add(i, MySpecialBonusList.Specials[i]);
    }
}
