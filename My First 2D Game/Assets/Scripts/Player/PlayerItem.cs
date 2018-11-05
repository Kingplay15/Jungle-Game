using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    UIController GameUIController;
    PlayerStats MyPlayerStats;
    PlayerDefense MyPlayerDefense;
    PlayerOffense MyPlayerOffense;
    Inventory PlayerInventory;
    ItemInstance PickedUpItem;

    //Bonus stats from items
    int[,] StatsFromItems = new int[,]
    {
        {0,0,0,0,0,0,0,0,0 }, //From weapon
        {0,0,0,0,0,0,0,0,0 }, //From shield
        {0,0,0,0,0,0,0,0,0 }, //From amulet
        {0,0,0,0,0,0,0,0,0 } //From ring
    };
    int[] TotalStatsBonus = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    [SerializeField] GameObject PlayerWeapon;
    SpriteRenderer PlayerWeaponSprite;

    [SerializeField] GameObject PlayerShield;
    SpriteRenderer PlayerShieldSprite;

    //For using quest items
    [SerializeField] List<GemHolder> GemHolders;

    void Awake()
    {
        MyPlayerStats = GetComponent<PlayerStats>();
        MyPlayerDefense = GetComponent<PlayerDefense>();
        MyPlayerOffense = GetComponent<PlayerOffense>();

        PlayerWeaponSprite = PlayerWeapon.GetComponent<SpriteRenderer>();
        PlayerShieldSprite = PlayerShield.GetComponent<SpriteRenderer>();

        GameUIController = FindObjectOfType<UIController>();
        PlayerInventory = FindObjectOfType<Inventory>();
    }

    void ApplySpecialBonus(int ItemSlot, List<int[]> Specials)
    {
        for (int i = 0; i < Specials.Count; i++)
            for (int j = 0; j < 9; j++)
                if (Specials[i][0] == j)
                    StatsFromItems[ItemSlot, j] += Specials[i][1];

        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 9; j++)
                TotalStatsBonus[j] += StatsFromItems[i, j];

        MyPlayerStats.UpdateStrength(TotalStatsBonus[0], TotalStatsBonus[3], TotalStatsBonus[4]);
        MyPlayerStats.UpdateAgility(TotalStatsBonus[1], TotalStatsBonus[5], TotalStatsBonus[6]);
        MyPlayerStats.UpdateIntelligence(TotalStatsBonus[2], TotalStatsBonus[7], TotalStatsBonus[8]);
    }

    public void UseConsumable(int ItemNum)
    {
        ConsumableItem ConsumableToUse;
        ItemDatabase.Instance.GetConsumables.TryGetValue(ItemNum, out ConsumableToUse);
        switch (ConsumableToUse.Name)
        {
            case "Health Potion":
                MyPlayerDefense.CurrentHealth += ConsumableToUse.UsingValue;
                if (MyPlayerDefense.CurrentHealth > PlayerStats.MaxHealth)
                    MyPlayerDefense.CurrentHealth = PlayerStats.MaxHealth;
                GameUIController.UpdateHealthBar();
                break;

            case "Mana Potion":
                MyPlayerOffense.CurrentMana += ConsumableToUse.UsingValue;
                if (MyPlayerOffense.CurrentMana > PlayerStats.MaxMana)
                    MyPlayerOffense.CurrentMana = PlayerStats.MaxMana;
                GameUIController.UpdateManaBar();
                break;
        }
    }

    public void EquipWeapon(int ItemNum, List<int[]> Specials, Sprite ItemImage)
    {
        for (int i = 0; i < 9; i++)
        {
            TotalStatsBonus[i] = 0;
            StatsFromItems[0, i] = 0;
        }

        Weapon WeaponToEquip;
        ItemDatabase.Instance.GetWeapons.TryGetValue(ItemNum, out WeaponToEquip);
        StatsFromItems[0, 3] = WeaponToEquip.Damage;
        StatsFromItems[0, 6] = WeaponToEquip.Speed;
        ApplySpecialBonus(0, Specials);
        PlayerWeaponSprite.sprite = ItemImage;
    }

    public void EquipShield(int ItemNum, List<int[]> Specials, Sprite ItemImage)
    {
        for (int i = 0; i < 9; i++)
        {
            TotalStatsBonus[i] = 0;
            StatsFromItems[1, i] = 0;
        }

        Shield ShieldToEquip;
        ItemDatabase.Instance.GetShields.TryGetValue(ItemNum, out ShieldToEquip);
        StatsFromItems[1, 5] = ShieldToEquip.Defense;
        ApplySpecialBonus(1, Specials);
        PlayerShieldSprite.sprite = ItemImage;
    }

    public void EquipJewelry(string ItemTag, int ItemNum, List<int[]> Specials, Sprite ItemImage)
    {
        for (int i = 0; i < 9; i++)
        {
            TotalStatsBonus[i] = 0;
            StatsFromItems[2, i] = 0;
            StatsFromItems[3, i] = 0;
        }

        if (ItemTag == "Amulet")
            ApplySpecialBonus(2, Specials);
        else ApplySpecialBonus(3, Specials); //Ring
    }

    public bool UseQuestItem(int ItemNum)
    {
        foreach (GemHolder element in GemHolders)
        {
            if (element.PlayerInRange && !element.AlreadyTriggered && ItemNum == element.HolderNum)
            {
                element.TriggerEvent();
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 14 && OtherObject.gameObject.tag != "Chest")  //Item layer (not chest)
        {
            PickedUpItem = OtherObject.GetComponent<ItemInstance>();

            if (OtherObject.gameObject.tag == "Gold") //Adding gold
            {
                PlayerInventory.AddGold(PickedUpItem.InstanceNumOrValue);
                SoundController.Instance.PlayGold();
                PickedUpItem.DestroyInstance();
            }

            else //Picking up item
            {
                if (PlayerInventory.IdentifyItem(PickedUpItem.InstanceNumOrValue,
                    PickedUpItem.InstanceSpecials, OtherObject.gameObject.tag))
                {
                    switch (OtherObject.gameObject.tag)
                    {
                        case "Consumable":
                            SoundController.Instance.PlayPotionPickedUp();
                            break;
                        case "Weapon":
                            SoundController.Instance.PlayWeapon();
                            break;
                        case "Shield":
                            SoundController.Instance.PlayShield();
                            break;
                    }
                    PickedUpItem.DestroyInstance();
                }
            }
        }
    }
}
