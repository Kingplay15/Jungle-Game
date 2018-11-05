using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    //Singleton
    public static SaveLoadController Instance = null;

    public PlayerData MainPlayerData { get; private set; }

    GameObject SaveSlot1;

    void Awake()
    {
        //There has to be only ONE instance of SaveLoadController in the game
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else if (Instance != this)
            Destroy(gameObject);

        MainPlayerData = new PlayerData();
    }

    public void Save()
    {
        //Save the game
        BinaryFormatter MyBF = new BinaryFormatter();
        FileStream SaveFile = File.Create(Application.persistentDataPath + "/PlayerData.dat");

        MainPlayerData.Class = PlayerClassName.PlayerClass;
        MainPlayerData.Name = PlayerClassName.PlayerName;

        MainPlayerData.Level = PlayerExperience.CurrentLevel;
        MainPlayerData.Experience = PlayerExperience.CurrentXP;

        MainPlayerData.StatPoints = StatPointAssignment.StatPoints;
        MainPlayerData.BaseStrength = PlayerStats.BaseStrength;
        MainPlayerData.TotalStrength = PlayerStats.TotalStrength;
        MainPlayerData.BaseAgility = PlayerStats.BaseAgility;
        MainPlayerData.TotalAgility = PlayerStats.TotalAgility;
        MainPlayerData.BaseIntelligence = PlayerStats.BaseIntelligence;
        MainPlayerData.TotalIntelligence = PlayerStats.TotalIntelligence;

        MainPlayerData.GoldAmount = Inventory.Instance.GoldAmount;
        MainPlayerData.InventorySlotNum = Inventory.ItemSlotNum;
        for (int i = 0; i < Inventory.ItemSlotNum; i++)
        {
            MainPlayerData.ContainedItemSlots.Add(new InventorySlotData());
            MainPlayerData.ContainedItemSlots[i].IsAvailale = Inventory.Instance.ItemSlots[i].IsAvailable;
            if (!Inventory.Instance.ItemSlots[i].IsAvailable)
            {
                int Count = 0;
                foreach (KeyValuePair<string, InventorySlot> element in InventorySlot.EquipmentPointers)
                {
                    Count++;
                    if (element.Value == Inventory.Instance.ItemSlots[i])
                    {
                        MainPlayerData.ContainedItemSlots[i].IsEquipped = true;
                        break;
                    }
                    else if (Count == 4)
                        MainPlayerData.ContainedItemSlots[i].IsEquipped = false;
                }

                MainPlayerData.ContainedItemSlots[i].ContainedItemNum = Inventory.Instance.ItemSlots[i].ContainedItemNum;
                MainPlayerData.ContainedItemSlots[i].ContainedItemTag = Inventory.Instance.ItemSlots[i].ContainedItemTag;
                MainPlayerData.ContainedItemSlots[i].ContainedStackNum = Inventory.Instance.ItemSlots[i].ContainedStackNum;
                MainPlayerData.ContainedItemSlots[i].ContainedItemSpecials = Inventory.Instance.ItemSlots[i].ContainedItemSpecials;
                MainPlayerData.ContainedItemSlots[i].ContainedItemRequirements = Inventory.Instance.ItemSlots[i].ContainedItemRequirements;
            }
        }

        MyBF.Serialize(SaveFile, MainPlayerData);
        SaveFile.Close();

        //Done saving
        UIController.Instance.SaveSuccessDisplay();
    }

    public void CheckSaveFile()
    {
        SaveSlot1 = GameObject.FindGameObjectWithTag("SaveSlot");

        if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {
            BinaryFormatter MyBF = new BinaryFormatter();
            FileStream SaveFile = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
            MainPlayerData = (PlayerData)MyBF.Deserialize(SaveFile);
            SaveFile.Close();
        }
        else SaveSlot1.SetActive(false);
    }

    public void LoadGame()
    {
        PlayerClassName.PlayerClass = MainPlayerData.Class;
        PlayerClassName.PlayerName = MainPlayerData.Name;

        PlayerExperience.CurrentLevel = MainPlayerData.Level;
        PlayerExperience.CurrentXP = MainPlayerData.Experience;

        StatPointAssignment.StatPoints = MainPlayerData.StatPoints;
        StatPointAssignment[] tempStatsAdding = new StatPointAssignment[3];
        tempStatsAdding = FindObjectsOfType<StatPointAssignment>();
        for (int i = 0; i < 3; i++)
        {
            tempStatsAdding[i].UpdateStatPoints();
        }

        PlayerStats.BaseStrength = MainPlayerData.BaseStrength;
        PlayerStats.TotalStrength = MainPlayerData.TotalStrength;
        PlayerStats.BaseAgility = MainPlayerData.BaseAgility;
        PlayerStats.TotalAgility = MainPlayerData.TotalAgility;
        PlayerStats.BaseIntelligence = MainPlayerData.BaseIntelligence;
        PlayerStats.TotalIntelligence = MainPlayerData.TotalIntelligence;

        Inventory.Instance.AddGold(MainPlayerData.GoldAmount);
        for (int i = 0; i < MainPlayerData.InventorySlotNum; i++)
        {
            if (!MainPlayerData.ContainedItemSlots[i].IsAvailale)
            {
                int tempNum = MainPlayerData.ContainedItemSlots[i].ContainedItemNum;
                string tempTag = MainPlayerData.ContainedItemSlots[i].ContainedItemTag;
                int tempStackNum = MainPlayerData.ContainedItemSlots[i].ContainedStackNum;
                List<int[]> tempSpecials = MainPlayerData.ContainedItemSlots[i].ContainedItemSpecials;
                int[] tempRequirements = MainPlayerData.ContainedItemSlots[i].ContainedItemRequirements;
                Inventory.Instance.ItemSlots[i].DisplayItem(tempNum, tempSpecials, tempTag, tempRequirements);
                Inventory.Instance.ItemSlots[i].StackItem(tempStackNum - 1);
                if (MainPlayerData.ContainedItemSlots[i].IsEquipped)
                    Inventory.Instance.ItemSlots[i].UseItem();
            }
        }
        PlayerDefense tempPlayerDef = FindObjectOfType<PlayerDefense>();
        tempPlayerDef.CurrentHealth = PlayerStats.MaxHealth;
        UIController.Instance.UpdateHealthBar();

        PlayerOffense tempPlayerOff = FindObjectOfType<PlayerOffense>();
        tempPlayerOff.CurrentMana = PlayerStats.MaxMana;
        UIController.Instance.UpdateManaBar();
    }
}

[Serializable]
public class InventorySlotData
{
    public bool IsAvailale;
    public bool IsEquipped = false;
    public int ContainedItemNum;
    public string ContainedItemTag;
    public int ContainedStackNum;
    public List<int[]> ContainedItemSpecials;
    public int[] ContainedItemRequirements;
}

[Serializable]
public class PlayerData
{
    //Class and name
    public string Class;
    public string Name;

    //Level and experience
    public int Level;
    public int Experience;

    //Stats
    public int StatPoints;
    public int BaseStrength;
    public int TotalStrength;
    public int BaseAgility;
    public int TotalAgility;
    public int BaseIntelligence;
    public int TotalIntelligence;

    //Inventory
    public int InventorySlotNum;
    public List<InventorySlotData> ContainedItemSlots = new List<InventorySlotData>();
    public int GoldAmount;
}
