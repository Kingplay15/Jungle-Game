using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    PlayerOffense MyPlayerOffense;
    PlayerDefense MyPlayerDefense;
    Inventory PlayerInventory;

    public static int TotalStrength { get; set; }
    int DefaultStrength = 3;
    public static int BaseStrength { get; set; }
    public int StrengthFromItem { get; set; }
    public static int DamageModifier = 2;
    public static int TotalDamage { get; set; }
    public int BaseDamage { get; set; }
    public int DamageFromItem { get; set; }
    public static int HealthModifier = 1;
    public static int MaxHealth { get; set; }
    int BaseHealth = 20;
    public int HealthFromItem { get; set; }
    [SerializeField] CharacterInfoDislay StrengthDisplay;

    public static int TotalAgility { get; set; }
    int DefaultAgility = 2;
    public static int BaseAgility { get; set; }
    public int AgilityFromItem { get; set; }
    public static int DefenseModifier = 3;
    public static int TotalDefense { get; set; }
    public int BaseDefense { get; set; }
    public int DefenseFromItem { get; set; }
    public static int AttackSpeedModifier = 2;
    public static int TotalAttackSpeed { get; set; }
    public int BaseAttackSpeed { get; set; }
    public int AttackSpeedFromItem { get; set; }
    [SerializeField] CharacterInfoDislay AgilityDisplay;

    public static int TotalIntelligence { get; set; }
    int DefaultInteligence = 1;
    public static int BaseIntelligence { get; set; }
    public int IntelligenceFromItem { get; set; }
    public static int SpellPowerModifier = 1;
    public static int TotalSpellPower { get; set; }
    public int BaseSpellPower { get; set; }
    public int SpellPowerFromItem { get; set; }
    public static int ManaModifier = 1;
    public static int MaxMana { get; set; }
    int BaseMana = 5;
    public int ManaFromItem { get; set; }
    [SerializeField] CharacterInfoDislay IntelligenceDisplay;

    void Awake()
    {
        MyPlayerOffense = FindObjectOfType<PlayerOffense>();
        MyPlayerDefense = FindObjectOfType<PlayerDefense>();

        if (SceneController.Instance.NewGameStarted)
        {
            BaseStrength = DefaultStrength;
            TotalStrength = BaseStrength;
            BaseDamage = BaseStrength * DamageModifier;
            MaxHealth = BaseStrength * HealthModifier + BaseHealth;
            MyPlayerDefense.CurrentHealth = MaxHealth;

            BaseAgility = DefaultAgility;
            TotalAgility = BaseAgility;
            BaseDefense = BaseAgility * DefenseModifier;
            BaseAttackSpeed = BaseAgility * AttackSpeedModifier;

            BaseIntelligence = DefaultInteligence;
            TotalIntelligence = BaseIntelligence;
            BaseSpellPower = BaseIntelligence * SpellPowerModifier;
            MaxMana = BaseIntelligence * ManaModifier + BaseMana;
            MyPlayerOffense.CurrentMana = MaxMana;
        }

        PlayerInventory = FindObjectOfType<Inventory>();
    }

    public void UpdateStrength(int StrengthBonus, int DamageBonus, int HealthBonus)
    {
        TotalStrength = BaseStrength + StrengthBonus;
        BaseDamage = TotalStrength * DamageModifier;
        StrengthFromItem = StrengthBonus;

        DamageFromItem = DamageBonus;
        TotalDamage = BaseDamage + DamageFromItem;

        HealthFromItem = HealthBonus;
        MaxHealth = BaseHealth + TotalStrength * HealthModifier + HealthFromItem;

        UIController.Instance.UpdateHealthBar();
        PlayerInventory.LoopItemRequirements();
        StrengthDisplay.UpdateStatsValue(TotalStrength, TotalDamage, MaxHealth);
    }

    public void UpdateAgility(int AgilityBonus, int DefenseBonus, int AttackSpeedBonus)
    {
        TotalAgility = BaseAgility + AgilityBonus;
        BaseDefense = TotalAgility * DefenseModifier;
        AgilityFromItem = AgilityBonus;

        DefenseFromItem = DefenseBonus;
        TotalDefense = BaseDefense + DefenseFromItem;

        BaseAttackSpeed = TotalAgility * AttackSpeedModifier;
        AttackSpeedFromItem = AttackSpeedBonus;

        TotalAttackSpeed = BaseAttackSpeed + AttackSpeedFromItem;
        PlayerInventory.LoopItemRequirements();
        AgilityDisplay.UpdateStatsValue(TotalAgility, TotalDefense, TotalAttackSpeed);
    }

    public void UpdateIntelligence(int IntelligenceBonus, int SpellPowerBonus, int ManaBonus)
    {
        TotalIntelligence = BaseIntelligence + IntelligenceBonus;
        BaseSpellPower = TotalIntelligence * SpellPowerModifier;
        IntelligenceFromItem = IntelligenceBonus;

        SpellPowerFromItem = SpellPowerBonus;
        TotalSpellPower = BaseSpellPower + SpellPowerFromItem;

        ManaFromItem = ManaBonus;
        MaxMana = BaseMana + TotalIntelligence * ManaModifier + ManaFromItem;

        UIController.Instance.UpdateManaBar();
        PlayerInventory.LoopItemRequirements();
        IntelligenceDisplay.UpdateStatsValue(TotalIntelligence, TotalSpellPower, MaxMana);
    }
}
