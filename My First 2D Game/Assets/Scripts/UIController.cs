using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance = null;

    public static bool IsAnyUIOn = false;
    PlayerDefense MyPlayerDefense;
    PlayerOffense MyPlayerOffense;

    //Inventory
    [SerializeField] GameObject Inventory;
    RectTransform InventoryRT;
    [SerializeField] GameObject ItemDescription;
    public static bool IsInventoryOff { get; private set; }

    //Character
    [SerializeField] GameObject Character;
    RectTransform CharacterRT;
    [SerializeField] GameObject CharacterDescription;

    //EscMenu
    [SerializeField] GameObject EscMenu;

    //Informer
    [SerializeField] GameObject Informer;
    Animator InformerAnim;
    int DisplayHarsh = Animator.StringToHash("Display");

    //Game Over
    [SerializeField] GameObject GameOverEffect;

    //Health bar
    [SerializeField] Slider HealthSlider;
    [SerializeField] Text HealthPortion;

    //Mana bar
    [SerializeField] Slider ManaSlider;
    [SerializeField] Text ManaPortion;

    void Awake()
    {
        //There has to be only ONE instance of UIController in the game
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        MyPlayerDefense = FindObjectOfType<PlayerDefense>();
        MyPlayerOffense = FindObjectOfType<PlayerOffense>();

        InventoryRT = Inventory.GetComponent<RectTransform>();
        CharacterRT = Character.GetComponent<RectTransform>();
        InformerAnim = Informer.GetComponent<Animator>();

        HealthSlider.maxValue = PlayerStats.MaxHealth;
        HealthSlider.value = PlayerStats.MaxHealth;
        HealthPortion.text = PlayerStats.MaxHealth + "/" + PlayerStats.MaxHealth;

        ManaSlider.maxValue = PlayerStats.MaxMana;
        ManaSlider.value = PlayerStats.MaxMana;
        ManaPortion.text = PlayerStats.MaxMana + "/" + PlayerStats.MaxMana;
    }

    void Update()
    {
        if (!MyPlayerDefense.Dead)
        {
            ToggleInventory();
            ToggleCharacter();
            ToggleEscMenu();
        }
    }

    void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
            if (InventoryRT.localScale.x == 0f)
            {
                if (IsAnyUIOn) //Only one UI can be displayed at a time
                    return;

                InventoryRT.localScale = new Vector3(1f, 1f, 0f);
                IsAnyUIOn = true;
                IsInventoryOff = false;
                Informer.SetActive(false);
                Cursor.visible = true;
                Time.timeScale = 0f; //Also pause the game
            }
            else
            {
                InventoryRT.localScale = new Vector3(0f, 0f, 0f);
                IsInventoryOff = true;
                Informer.SetActive(true);
                Cursor.visible = false;
                ResetUISettings();
            }
    }

    void ToggleCharacter()
    {
        if (Input.GetKeyDown(KeyCode.C))
            if (CharacterRT.localScale.x == 0f)
            {
                if (IsAnyUIOn) //Only one UI can be displayed at a time
                    return;

                CharacterRT.localScale = new Vector3(1f, 1f, 0f);
                IsAnyUIOn = true;
                Informer.SetActive(false);
                Cursor.visible = true;
                Time.timeScale = 0f; //Also pause the game
            }
            else
            {
                CharacterRT.localScale = new Vector3(0f, 0f, 0f);
                Informer.SetActive(true);
                Cursor.visible = false;
                ResetUISettings();
            }
    }

    public void ToggleEscMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (!EscMenu.activeInHierarchy)
            {
                if (IsAnyUIOn) //Only one UI can be displayed at a time
                    return;

                EscMenu.SetActive(true);
                IsAnyUIOn = true;
                Informer.SetActive(false);
                Cursor.visible = true;
                Time.timeScale = 0f; //Also pause the game
            }
            else EscMenuOff();
    }

    void EscMenuOff()
    {
        EscMenu.SetActive(false);
        Informer.SetActive(true);
        Cursor.visible = false;
        ResetUISettings();
    }

    public void ToggleItemDescription(bool Condition)
    {
        if (Condition)
        {
            ItemDescription.gameObject.SetActive(true);
        }

        else ItemDescription.gameObject.SetActive(false);
    }

    public void ToggleCharacterDescription(bool Condition)
    {
        if (Condition)
        {
            CharacterDescription.gameObject.SetActive(true);
        }

        else CharacterDescription.gameObject.SetActive(false);
    }

    public void SaveSuccessDisplay()
    {
        EscMenuOff();
        InformerDisplay("CHARACTER SAVED SUCCESSFULLY");
    }

    public void InformerDisplay(string Detail)
    {
        Informer.GetComponentInChildren<Text>().text = Detail;
        InformerAnim.SetTrigger(DisplayHarsh);
    }

    public static void ResetUISettings()
    {
        Time.timeScale = 1f;
        IsAnyUIOn = false;
    }

    public void DisplayGameOver(int GameOverCase)
    {
        GameOverEffect.SetActive(true);
        GameOverEffect.GetComponentInChildren<GameOver>().GameOverCase = GameOverCase;
        IsAnyUIOn = true;
    }

    public void UpdateHealthBar()
    {
        HealthSlider.maxValue = PlayerStats.MaxHealth;
        HealthSlider.value = MyPlayerDefense.CurrentHealth;
        HealthPortion.text = HealthSlider.value + "/" + HealthSlider.maxValue;
    }

    public void UpdateManaBar()
    {
        ManaSlider.maxValue = PlayerStats.MaxMana;
        ManaSlider.value = MyPlayerOffense.CurrentMana;
        ManaPortion.text = ManaSlider.value + "/" + ManaSlider.maxValue;
    }
}
