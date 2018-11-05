using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] InfoDisplay ItemDescription;
    PlayerItem ItemUsing;

    public bool IsAvailable { get; private set; } = true;

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

    //Quests
    [SerializeField] List<Sprite> QuestSpritesList; //Create this because Dictionary cannot be serialied
    Dictionary<int, Sprite> QuestSpritesDict;

    //For displaying item
    [SerializeField] Image ItemIcon;
    [SerializeField] Image MovingItemDisplay;
    Sprite ImageToLoad;
    Button MyButton;
    Color DefaultHighlighted;

    // For saving the item information
    public int ContainedItemNum { get; private set; } = 0;
    public string ContainedItemTag { get; private set; } = "";
    public List<int[]> ContainedItemSpecials { get; private set; }
    public int[] ContainedItemRequirements { get; private set; }
    bool UnableEquipping = false;

    //For stacking item
    public int ContainedStackNum { get; private set; } = 0;
    public int MaxStackingNum { get; private set; } = 10;
    public bool IsFull { get; private set; } = false;
    [SerializeField] Image ContainedStackNumBorder;
    [SerializeField] Text ContainedStackNumDisplay;

    //For equipping item
    [SerializeField] Image ContainedEquipIconBorder;
    public static Dictionary<string, InventorySlot> EquipmentPointers;
    int[] PlayerCurrentStats;

    //For character UI
    [SerializeField] CharacterInfoDislay WeaponSlot;
    [SerializeField] CharacterInfoDislay ShieldSlot;
    [SerializeField] CharacterInfoDislay AmuletSlot;
    [SerializeField] CharacterInfoDislay RingSlot;

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

        QuestSpritesDict = new Dictionary<int, Sprite>();
        for (int i = 0; i < QuestSpritesList.Count; i++)
            QuestSpritesDict.Add(i, QuestSpritesList[i]);

        EquipmentPointers = new Dictionary<string, InventorySlot>()
        {
            { "Weapon", null},
            { "Shield", null},
            { "Amulet", null },
            { "Ring", null }
        };

        MyButton = GetComponent<Button>();
        DefaultHighlighted = MyButton.colors.highlightedColor;

        ItemUsing = FindObjectOfType<PlayerItem>();
    }

    public void DisplayItem(int ItemNum, List<int[]> Specials, string ItemTag, int[] Requirements)
    {
        if (ItemTag != "Consumable")
            IsFull = true;
        ContainedItemNum = ItemNum;
        ContainedStackNum = 1;
        ContainedItemTag = ItemTag;
        ContainedItemSpecials = new List<int[]>(Specials);
        switch (ItemTag)
        {
            case "Consumable":
                ConsumableSpritesDict.TryGetValue(ItemNum, out ImageToLoad);
                break;

            case "Weapon":
                WeaponSpritesDict.TryGetValue(ItemNum, out ImageToLoad);
                ContainedItemRequirements = Requirements;
                CheckItemRequirement();
                break;

            case "Shield":
                ShieldSpritesDict.TryGetValue(ItemNum, out ImageToLoad);
                ContainedItemRequirements = Requirements;
                CheckItemRequirement();
                break;

            case "Amulet":
                AmuletSpritesDict.TryGetValue(ItemNum, out ImageToLoad);
                ContainedItemRequirements = Requirements;
                CheckItemRequirement();
                break;

            case "Ring":
                RingSpritesDict.TryGetValue(ItemNum, out ImageToLoad);
                ContainedItemRequirements = Requirements;
                CheckItemRequirement();
                break;

            case "Quest":
                QuestSpritesDict.TryGetValue(ItemNum, out ImageToLoad);
                break;
        }

        if (!ItemIcon.gameObject.activeInHierarchy)
            ItemIcon.gameObject.SetActive(true);
        ItemIcon.sprite = ImageToLoad;
        IsAvailable = false;
    }

    public void StackItem(int Quantity)
    {
        ContainedStackNum += Quantity;
        if (ContainedStackNum == 0)
        {
            ItemIcon.gameObject.SetActive(false);
            ResetSlotInfo();
        }

        else if (ContainedStackNum < 2)
            ContainedStackNumBorder.gameObject.SetActive(false);

        else ContainedStackNumBorder.gameObject.SetActive(true);

        if (ContainedStackNum == MaxStackingNum)
            IsFull = true;
        ContainedStackNumDisplay.text = ContainedStackNum.ToString();
    }

    void ResetSlotInfo()
    {
        ContainedItemNum = 0;
        ContainedItemTag = "";
        ContainedStackNum = 0;
        ContainedItemSpecials.Clear();
        ContainedItemRequirements = null;
        UnableEquipping = false;
        ColorBlock MyColorBlock = MyButton.colors;
        MyColorBlock.highlightedColor = DefaultHighlighted;
        MyButton.colors = MyColorBlock;
        IsAvailable = true;
        IsFull = false;
    }

    public void TakeOutItem()
    {
        if (Inventory.IsConsideringDiscarding)
            return;

        if (!IsAvailable) //Already contained item
        {
            if (!Inventory.IsMovingItem) //Taking the item out of the slot
            {
                if (ContainedEquipIconBorder.gameObject.activeInHierarchy)
                {
                    ContainedEquipIconBorder.gameObject.SetActive(false);
                    MovingItemInfo.IsContainingEquippedItem = true;
                }

                Inventory.IsMovingItem = true;
                MovingItemInfo.ItemNum = ContainedItemNum;
                MovingItemInfo.StackNum = ContainedStackNum;
                MovingItemInfo.ItemTag = ContainedItemTag;
                MovingItemInfo.ItemSpecials = new List<int[]>(ContainedItemSpecials);
                MovingItemInfo.ItemRequirements = ContainedItemRequirements;
                ResetSlotInfo();
                ItemIcon.gameObject.SetActive(false);
                ContainedStackNumBorder.gameObject.SetActive(false);
                MovingItemDisplay.gameObject.SetActive(true);
                if (MovingItemDisplay.sprite != ImageToLoad)
                    MovingItemDisplay.sprite = ImageToLoad;
            }

            else ExchangeItem();
        }

        //The slot is empty
        else if (Inventory.IsMovingItem)
            DropItem();
    }

    public void DropItem()
    {
        //Dropping the item into a new slot 
        if (MovingItemInfo.IsContainingEquippedItem)
        {
            ContainedEquipIconBorder.gameObject.SetActive(true);
            EquipmentPointers[MovingItemInfo.ItemTag] = gameObject.GetComponent<InventorySlot>();
            MovingItemInfo.IsContainingEquippedItem = false;
        }

        MovingItemDisplay.gameObject.SetActive(false);
        Inventory.IsMovingItem = false;
        DisplayItem(MovingItemInfo.ItemNum, MovingItemInfo.ItemSpecials,
            MovingItemInfo.ItemTag, MovingItemInfo.ItemRequirements);
        StackItem(MovingItemInfo.StackNum - 1);
    }

    public void ExchangeItem()
    {
        //Stacking item
        if (MovingItemDisplay.sprite == ItemIcon.sprite && ContainedItemTag == "Consumable")
        {
            if (!IsFull)
            {
                if (MovingItemInfo.StackNum + ContainedStackNum <= MaxStackingNum)
                {
                    StackItem(MovingItemInfo.StackNum);
                    Inventory.IsMovingItem = false;
                    MovingItemDisplay.gameObject.SetActive(false);
                    return;
                }

                else
                {
                    int NumberToStack = MaxStackingNum - ContainedStackNum;
                    StackItem(NumberToStack);
                    MovingItemInfo.StackNum -= NumberToStack;
                    return;
                }
            }

            else return;
        }

        if (MovingItemInfo.IsContainingEquippedItem)
        {
            //Exchanging two equipped items
            if (ContainedEquipIconBorder.gameObject.activeInHierarchy)
                EquipmentPointers[MovingItemInfo.ItemTag] = gameObject.GetComponent<InventorySlot>();

            //Dropping an equipped item into and taking a normal one out
            else
            {
                ContainedEquipIconBorder.gameObject.SetActive(true);
                EquipmentPointers[MovingItemInfo.ItemTag] = gameObject.GetComponent<InventorySlot>();
                MovingItemInfo.IsContainingEquippedItem = false;
            }
        }

        //Dropping a normal item into and taking an equipped one out
        else if (ContainedEquipIconBorder.gameObject.activeInHierarchy)
        {
            ContainedEquipIconBorder.gameObject.SetActive(false);
            MovingItemInfo.IsContainingEquippedItem = true;
        }

        //Saving the moving item infos
        int tempNum = MovingItemInfo.ItemNum;
        int tempStack = MovingItemInfo.StackNum;
        string tempTag = MovingItemInfo.ItemTag;
        List<int[]> tempSpecials = new List<int[]>(MovingItemInfo.ItemSpecials);
        int[] tempRequirements = MovingItemInfo.ItemRequirements;

        //Taking out the current item in the slot
        MovingItemInfo.ItemNum = ContainedItemNum;
        MovingItemInfo.StackNum = ContainedStackNum;
        MovingItemInfo.ItemTag = ContainedItemTag;
        MovingItemInfo.ItemSpecials = new List<int[]>(ContainedItemSpecials);
        MovingItemInfo.ItemRequirements = ContainedItemRequirements;
        MovingItemDisplay.sprite = ItemIcon.sprite;

        //Dropping the moving item into the slot
        DisplayItem(tempNum, tempSpecials, tempTag, tempRequirements);
        StackItem(tempStack - 1);
    }

    public void Highlighted()
    {
        ColorBlock MyColorBlock = MyButton.colors;
        if (UnableEquipping)
        {
            MyColorBlock.highlightedColor = new Color(1, 0, 0);
            MyButton.colors = MyColorBlock;
        }
        else
        {
            MyColorBlock.highlightedColor = DefaultHighlighted;
            MyButton.colors = MyColorBlock;
        }

        UIController.Instance.ToggleItemDescription(!IsAvailable && !Inventory.IsMovingItem);
        switch (ContainedItemTag)
        {
            case "Consumable":
                ItemDescription.DisplayConsumableInfo(ContainedItemNum);
                break;

            case "Quest":
                ItemDescription.DisplayQuestItemInfo(ContainedItemNum);
                break;

            case "Weapon":
                ItemDescription.DisplayWeaponInfo(ContainedItemNum, ContainedItemSpecials, UnableEquipping);
                break;

            case "Shield":
                ItemDescription.DisplayShieldInfo(ContainedItemNum, ContainedItemSpecials, UnableEquipping);
                break;

            case "Amulet":
                ItemDescription.DisplayJewelryInfo(ContainedItemTag, ContainedItemNum,
                    ContainedItemSpecials, UnableEquipping);
                break;

            case "Ring":
                ItemDescription.DisplayJewelryInfo(ContainedItemTag, ContainedItemNum,
                    ContainedItemSpecials, UnableEquipping);
                break;
        }
    }

    public void Dehighlighted()
    {
        UIController.Instance.ToggleItemDescription(false);
    }

    public void CheckItemRequirement()
    {
        if (ContainedItemTag == "Consumable" || ContainedItemTag == "Quest")
            return;

        PlayerCurrentStats = new int[]
        {
            PlayerExperience.CurrentLevel,
            PlayerStats.TotalStrength,
            PlayerStats.TotalAgility,
            PlayerStats.TotalIntelligence
        };

        for (int i = 0; i < 4; i++)
            if (PlayerCurrentStats[i] < ContainedItemRequirements[i])
            {
                UnableEquipping = true;
                return;
            }
            else if (i == 3)
                UnableEquipping = false;
    }

    public void UseItem()
    {
        if (!IsAvailable && !Inventory.IsMovingItem)
        {
            if (ContainedItemTag == "")           
                return;
            
            else if (ContainedItemTag == "Consumable")
            {
                ItemUsing.UseConsumable(ContainedItemNum);
                StackItem(-1);
                SoundController.Instance.PlayPotionUsed();
            }

            else if (ContainedItemTag == "Quest")
            {
                if (ItemUsing.UseQuestItem(ContainedItemNum))
                {
                    SoundController.Instance.PlayGold();
                    StackItem(-1);
                }                 
            }

            else
            {
                //If the weapon/shield is not equipped and can be equipped...
                if (EquipmentPointers[ContainedItemTag] != gameObject.GetComponent<InventorySlot>() &&
                    !UnableEquipping)
                {
                    //..then equip it
                    if (ContainedItemTag == "Weapon")
                    {
                        ItemUsing.EquipWeapon(ContainedItemNum, ContainedItemSpecials, ImageToLoad);
                        WeaponSlot.EquipmentTag = ContainedItemTag;
                        WeaponSlot.EquipmentNum = ContainedItemNum;
                        WeaponSlot.EquipmentSpecials = ContainedItemSpecials;
                        WeaponSlot.EquipmentIcon.sprite = ImageToLoad;
                        SoundController.Instance.PlayWeapon();
                    }

                    else if (ContainedItemTag == "Shield")
                    {
                        ItemUsing.EquipShield(ContainedItemNum, ContainedItemSpecials, ImageToLoad);
                        ShieldSlot.EquipmentTag = ContainedItemTag;
                        ShieldSlot.EquipmentNum = ContainedItemNum;
                        ShieldSlot.EquipmentSpecials = ContainedItemSpecials;
                        ShieldSlot.EquipmentIcon.sprite = ImageToLoad;
                        SoundController.Instance.PlayShield();
                    }

                    else
                    {
                        ItemUsing.EquipJewelry(ContainedItemTag, ContainedItemNum, ContainedItemSpecials, ImageToLoad);
                        //First time equipping jewelry

                        if (ContainedItemTag == "Amulet")
                        {
                            AmuletSlot.EquipmentTag = ContainedItemTag;
                            AmuletSlot.EquipmentNum = ContainedItemNum;
                            AmuletSlot.EquipmentSpecials = ContainedItemSpecials;
                            AmuletSlot.EquipmentIcon.sprite = ImageToLoad;
                            if (EquipmentPointers[ContainedItemTag] == null)
                                AmuletSlot.EquipmentIcon.color = new Color(1f, 1f, 1f, 1f);
                        }
                        else //Ring
                        {
                            RingSlot.EquipmentTag = ContainedItemTag;
                            RingSlot.EquipmentNum = ContainedItemNum;
                            RingSlot.EquipmentSpecials = ContainedItemSpecials;
                            RingSlot.EquipmentIcon.sprite = ImageToLoad;
                            if (EquipmentPointers[ContainedItemTag] == null)
                                RingSlot.EquipmentIcon.color = new Color(1f, 1f, 1f, 1f);
                        }
                    }

                    ContainedEquipIconBorder.gameObject.SetActive(true);
                    if (EquipmentPointers[ContainedItemTag] != null) //If this item is not the first one to be equipped
                    {
                        EquipmentPointers[ContainedItemTag].ContainedEquipIconBorder.gameObject.SetActive(false);
                        EquipmentPointers[ContainedItemTag] = gameObject.GetComponent<InventorySlot>();
                    }
                    else EquipmentPointers[ContainedItemTag] = gameObject.GetComponent<InventorySlot>();
                }
            }
        }
    }
}
