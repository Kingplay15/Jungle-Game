using System;
using System.Collections.Generic;

[Serializable]
public class Item
{
    public int Number;
    public string Name;
    public int[] Requirement;
    public string Description;
    public int SellingValue;
}

[Serializable]
public class ConsumableItem : Item
{
    public int UsingValue;
}

[Serializable]
public class Weapon: Item
{
    public int Damage;
    public int Speed;
}

[Serializable]
public class Shield : Item
{
    public int Defense;
}

[Serializable]
public class Jewelry : Item
{
    public List<int> FixedSpecials = new List<int>();
}

[Serializable]
public class ItemList
{
    public List<ConsumableItem> Consumables = new List<ConsumableItem>();
    public List<Weapon> Weapons = new List<Weapon>();
    public List<Shield> Shields = new List<Shield>();
    public List<Jewelry> Amulets = new List<Jewelry>();
    public List<Jewelry> Rings = new List<Jewelry>();
    public List<Item> Quests = new List<Item>();
}

[Serializable]
public class SpecialBonus
{
    public int Number;
    public string Description;
}

[Serializable]
public class SpecialBonusList
{
    public List<SpecialBonus> Specials = new List<SpecialBonus>();
}

