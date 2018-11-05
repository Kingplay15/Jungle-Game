using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance = null;

    bool IsFull = false;
    public static bool IsMovingItem = false;
    public static bool IsConsideringDiscarding = false;
    Vector3 MousePosition;

    //For displaying default items
    PlayerOffense MyPlayerOffense;
    PlayerDefense MyPlayerDefense;
    bool DoneDefaultDisplaying = false;

    public static int ItemSlotNum { get; private set; }
    public List<InventorySlot> ItemSlots;
    [SerializeField] Image ConfirmationBG;
    [SerializeField] Image DenyDiscardingBG;
    [SerializeField] Image MovingItemDisplay;

    //Displaying gold
    public int GoldAmount { get; private set; } = 0;
    [SerializeField] Text GoldAmountDisplay;

    void Awake()
    {
        //There has to be only ONE instance of Inventory in the game
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        MyPlayerOffense = FindObjectOfType<PlayerOffense>();
        MyPlayerDefense = FindObjectOfType<PlayerDefense>();

        ItemSlotNum = ItemSlots.Count;
    }

    void Start()
    {
        if (SceneController.Instance.NewGameStarted)
        {
            List<int[]> DefaultSpecials = new List<int[]>(); //Default items have no special bonus
                                                             //Displaying default weapon
            IdentifyItem(MyPlayerOffense.DefaultWeaponNum, DefaultSpecials, "Weapon");
            //Displaying default shield
            IdentifyItem(MyPlayerDefense.DefaultShieldNum, DefaultSpecials, "Shield");
            DoneDefaultDisplaying = true;

            GoldAmountDisplay.text = GoldAmount.ToString();
        }
    }

    void Update()
    {
        MousePosition = Input.mousePosition;

        if (UIController.IsInventoryOff && IsMovingItem)
        {
            foreach (InventorySlot element in ItemSlots)
                if (element.IsAvailable)
                {
                    element.DropItem();
                    break;
                }
        }

        if (IsConsideringDiscarding)
        {
            if (Input.GetKeyDown(KeyCode.Return)) //Use Enter key to confirm...
            {
                if (DenyDiscardingBG.gameObject.activeInHierarchy)
                    ConfirmDeninal(); //...the game's denial...
                else DiscardItemYes(); //...or the player's acceptance of discarding items
            }

            else if (Input.GetKeyDown(KeyCode.Escape)) //Use Esc key to cancel discarding items
                DiscardItemNo();
        }
    }

    void LateUpdate()
    {
        if (IsMovingItem)
            MovingItemDisplay.rectTransform.position = MousePosition;
    }

    public bool IdentifyItem(int ItemNum, List<int[]> Specials, string ItemTag)
    {
        if (ItemTag == "Consumable")
        {
            ConsumableItem NewItem;
            ItemDatabase.Instance.GetConsumables.TryGetValue(ItemNum, out NewItem);
            return CheckItem(NewItem, ItemNum, Specials, ItemTag);
        }

        else if (ItemTag == "Weapon")
        {
            Weapon NewItem;
            ItemDatabase.Instance.GetWeapons.TryGetValue(ItemNum, out NewItem);
            return CheckItem(NewItem, ItemNum, Specials, ItemTag);
        }

        else if (ItemTag == "Shield")
        {
            Shield NewItem;
            ItemDatabase.Instance.GetShields.TryGetValue(ItemNum, out NewItem);
            return CheckItem(NewItem, ItemNum, Specials, ItemTag);
        }

        else if (ItemTag == "Amulet")
        {
            Jewelry NewItem;
            ItemDatabase.Instance.GetAmulets.TryGetValue(ItemNum, out NewItem);
            return CheckItem(NewItem, ItemNum, Specials, ItemTag);
        }

        else if (ItemTag == "Ring")
        {
            Jewelry NewItem;
            ItemDatabase.Instance.GetRings.TryGetValue(ItemNum, out NewItem);
            return CheckItem(NewItem, ItemNum, Specials, ItemTag);
        }

        else if (ItemTag == "Quest")
        {
            Item NewItem;
            ItemDatabase.Instance.GetQuests.TryGetValue(ItemNum, out NewItem);
            return CheckItem(NewItem, ItemNum, Specials, ItemTag);
        }

        else return false;
    }

    bool CheckItem(Item PickedUpItem, int ItemNum, List<int[]> Specials, string ItemTag)
    {
        if (PickedUpItem == null) //Item is not found
        {
            Debug.Log("This item has not been found in the database");
            return false;
        }

        else //Found item in the database
        {
            AddItem(ItemNum, Specials, ItemTag, PickedUpItem.Requirement);
            if (IsFull)
            {
                UIController.Instance.InformerDisplay("Inventory is full");
                IsFull = false;
                return false;
            }
            else
            {
                if (DoneDefaultDisplaying)
                {
                    UIController.Instance.InformerDisplay(PickedUpItem.Name + " picked up");
                }
            }
            return true;
        }
    }

    void AddItem(int Num, List<int[]> Specials, string Tag, int[] Requirements)
    {
        for (int i = 0; i < ItemSlots.Count; i++)
            if (ItemSlots[i].IsAvailable) //Adding item
            {
                ItemSlots[i].DisplayItem(Num, Specials, Tag, Requirements);
                if (!DoneDefaultDisplaying)
                    ItemSlots[i].UseItem();
                break;
            }

            else //Stacking item
            {
                if (Num == ItemSlots[i].ContainedItemNum && Tag == ItemSlots[i].ContainedItemTag &&
                    !ItemSlots[i].IsFull)
                {
                    ItemSlots[i].StackItem(1);
                    break;
                }

                else if (i == ItemSlots.Count - 1) //Inventory is full
                    IsFull = true;
            }
    }

    public void ConfirmDiscarding()
    {
        if (!IsMovingItem)
            return;

        IsMovingItem = false;
        IsConsideringDiscarding = true;

        if (MovingItemInfo.IsContainingEquippedItem)
        {
            DenyDiscardingBG.rectTransform.position = MousePosition + new Vector3(-20f, 50f, 0f);
            DenyDiscardingBG.gameObject.SetActive(true);
        }

        else
        {
            ConfirmationBG.rectTransform.position = MousePosition + new Vector3(-20f, 50f, 0f);
            ConfirmationBG.gameObject.SetActive(true);
        }
    }

    public void ConfirmDeninal()
    {
        DenyDiscardingBG.gameObject.SetActive(false);
        IsMovingItem = true;
        IsConsideringDiscarding = false;
    }

    public void DiscardItemYes()
    {
        ConfirmationBG.gameObject.SetActive(false);
        MovingItemDisplay.gameObject.SetActive(false);
        IsMovingItem = false;
        IsConsideringDiscarding = false;
    }

    public void DiscardItemNo()
    {
        ConfirmationBG.gameObject.SetActive(false);
        IsMovingItem = true;
        IsConsideringDiscarding = false;
    }

    public void LoopItemRequirements()
    {
        foreach (InventorySlot element in ItemSlots)
            if (!element.IsAvailable && element.ContainedItemTag != "Consumable")
                element.CheckItemRequirement();
    }

    public void AddGold(int GoldValue)
    {
        GoldAmount += GoldValue;
        if (GoldValue == 0)
            GoldAmountDisplay.text = GoldAmount.ToString();
        else
        {
            GoldAmountDisplay.text = GoldAmount.ToString("#,###");
            UIController.Instance.InformerDisplay("+" + GoldValue + " gold");
        }
    }
}
