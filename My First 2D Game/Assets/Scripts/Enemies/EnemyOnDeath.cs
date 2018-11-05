using System.Collections.Generic;
using UnityEngine;

public class EnemyOnDeath : MonoBehaviour
{
    public int XPReward { get; private set; }

    //Fields for dropping item on death
    [SerializeField] ItemInstance DropInstance;
    //Consumable Items
    [SerializeField] List<Sprite> ConsumableSpritesList; //Create this because Dictionary cannot be serialied
    Dictionary<int, Sprite> ConsumableSpritesDict;

    //Weapons
    [SerializeField] List<Sprite> WeaponSpritesList; //Create this because Dictionary cannot be serialied
    Dictionary<int, Sprite> WeaponSpritesDict;

    //Shields
    [SerializeField] List<Sprite> ShieldSpritesList; //Create this because Dictionary cannot be serialied
    Dictionary<int, Sprite> ShieldSpritesDict;

    //Amulets
    [SerializeField] List<Sprite> AmuletSpritesList; //Create this because Dictionary cannot be serialied
    Dictionary<int, Sprite> AmuletSpritesDict;

    //Rings
    [SerializeField] List<Sprite> RingSpritesList; //Create this because Dictionary cannot be serialied
    Dictionary<int, Sprite> RingSpritesDict;

    protected int DropTypeSeedNum;
    protected int GoldChance = 30; //30% chance of dropping gold
    protected int ConsumablesChance = 60; //30% chance of dropping consumable items, 
    protected int EquipmentsChance = 80; //20% chance of dropping equipments, 20% left of dropping nothing

    [SerializeField] GameObject[] GoldType;
    protected int GoldTypeSeedNum;
    protected int DiamondChance = 20; //20% chance of dropping diamond
    protected int GoldCoinChance = 60; ////40% chance of dropping gold coin, 40% left is for dropping silver coin

    void Awake()
    {
        ConsumableSpritesDict = new Dictionary<int, Sprite>();
        for (int i = 0; i < ConsumableSpritesList.Count; i++)
            ConsumableSpritesDict.Add(i, ConsumableSpritesList[i]);

        WeaponSpritesDict = new Dictionary<int, Sprite>();
        for (int i = 0; i < WeaponSpritesList.Count; i++)
            WeaponSpritesDict.Add(i, WeaponSpritesList[i]);

        ShieldSpritesDict = new Dictionary<int, Sprite>();
        for (int i = 0; i < ShieldSpritesList.Count; i++)
            ShieldSpritesDict.Add(i, ShieldSpritesList[i]);

        AmuletSpritesDict = new Dictionary<int, Sprite>();
        for (int i = 0; i < AmuletSpritesList.Count; i++)
            AmuletSpritesDict.Add(i, AmuletSpritesList[i]);

        RingSpritesDict = new Dictionary<int, Sprite>();
        for (int i = 0; i < RingSpritesList.Count; i++)
            RingSpritesDict.Add(i, RingSpritesList[i]);

        switch (tag)
        {
            case "Bee":
                XPReward = 1;
                break;
            case "Chomper":
                XPReward = 5;
                break;
            case "Snake":
                XPReward = 7;
                break;
            case "Bear":
                XPReward = 10;
                break;
        }

        DropTypeSeedNum = Random.Range(1, 101);
        GoldTypeSeedNum = Random.Range(1, 101);
        if (gameObject.tag == "Bee") //Bee can only drop silver coin
        {
            DropTypeSeedNum = GoldChance;
            GoldTypeSeedNum = 61;
        }
    }

    public void DropItem()
    {
        int ConsumablesSeedNum = Random.Range(1, 101);
        int EquipmentSeedNum = Random.Range(1, 101);

        if (DropTypeSeedNum <= GoldChance) //Dropping gold
        {
            if (GoldTypeSeedNum <= DiamondChance)
                Instantiate(GoldType[0], transform.position, Quaternion.identity);
            else if (GoldTypeSeedNum <= GoldCoinChance)
                Instantiate(GoldType[1], transform.position, Quaternion.identity);
            else Instantiate(GoldType[2], transform.position, Quaternion.identity);
        }

        else
        {
            if (DropTypeSeedNum > EquipmentsChance)
                return;

            ItemInstance EnemyDrop = Instantiate(DropInstance, transform.position, Quaternion.identity);
            Sprite DropImage;

            if (DropTypeSeedNum <= ConsumablesChance) //Dropping consumable item
            {
                EnemyDrop.tag = "Consumable";
                if (ConsumablesSeedNum <= 70) //70% chance of dropping health potion
                {
                    ConsumableSpritesDict.TryGetValue(0, out DropImage);
                    EnemyDrop.GetComponentInChildren<SpriteRenderer>().sprite = DropImage;
                    EnemyDrop.InstanceNumOrValue = 0;
                }
                else //30% chance of dropping mana potion
                {
                    ConsumableSpritesDict.TryGetValue(1, out DropImage);
                    EnemyDrop.GetComponentInChildren<SpriteRenderer>().sprite = DropImage;
                    EnemyDrop.InstanceNumOrValue = 1;
                }
            }
            else if (DropTypeSeedNum <= EquipmentsChance) //Dropping equipment
            {
                if (EquipmentSeedNum <= 30) //30% chance of dropping weapon
                {
                    EnemyDrop.tag = "Weapon";
                    int WeaponSeedNum = Random.Range(0, 9);
                    WeaponSpritesDict.TryGetValue(WeaponSeedNum, out DropImage);
                    EnemyDrop.GetComponentInChildren<SpriteRenderer>().sprite = DropImage;
                    EnemyDrop.InstanceNumOrValue = WeaponSeedNum;
                }
                else if (EquipmentSeedNum <= 70)  //40% chance of dropping shield
                {
                    EnemyDrop.tag = "Shield";
                    int ShieldSeedNum = Random.Range(0, 9);
                    ShieldSpritesDict.TryGetValue(ShieldSeedNum, out DropImage);
                    EnemyDrop.GetComponentInChildren<SpriteRenderer>().sprite = DropImage;
                    EnemyDrop.InstanceNumOrValue = ShieldSeedNum;
                }
                else  //30% chance of dropping jewelry
                {
                    int JewelrySeedNum = Random.Range(0, 2); //Can only drop normal jewelry
                    if (EquipmentSeedNum <= 85) //15% for dropping amulet
                    {
                        AmuletSpritesDict.TryGetValue(JewelrySeedNum, out DropImage);
                        EnemyDrop.tag = "Amulet";
                    }
                    else
                    {
                        RingSpritesDict.TryGetValue(JewelrySeedNum, out DropImage);
                        EnemyDrop.tag = "Ring"; //15% left for ring
                    }
                    EnemyDrop.GetComponentInChildren<SpriteRenderer>().sprite = DropImage;
                    EnemyDrop.InstanceNumOrValue = JewelrySeedNum;
                }
            }
            EnemyDrop.RandomizeSpecials();
        }
    }
}
