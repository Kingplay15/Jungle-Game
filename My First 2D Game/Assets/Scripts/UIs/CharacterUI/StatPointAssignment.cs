using UnityEngine;
using UnityEngine.UI;

public class StatPointAssignment : MonoBehaviour
{
    PlayerStats MyPlayerStats;
    public static int StatPoints { get; set; } = 0;
    [SerializeField] StatPointAssignment[] OtherStats;
    [SerializeField] Text StatPointsDisplay;
    public RectTransform MyRect { get; set; }

    void Awake()
    {
        MyRect = GetComponent<RectTransform>();
        MyPlayerStats = FindObjectOfType<PlayerStats>();
    }

    void GeneralUpdate()
    {
        StatPoints--;
        UpdateStatPoints();

    }

    public void UpdateStatPoints()
    {
        StatPointsDisplay.text = StatPoints.ToString();
        if (StatPoints == 0) //Spending stat points
        {
            MyRect.localScale = new Vector3(0f, 1f, 1f);
            foreach (StatPointAssignment element in OtherStats)
                element.MyRect.localScale = new Vector3(0f, 1f, 1f);
        }
        else if (MyRect.localScale.x == 0f) //Gaining stat points
            MyRect.localScale = new Vector3(1f, 1f, 1f);
    }

    public void AddStrength()
    {
        PlayerStats.BaseStrength++;
        MyPlayerStats.UpdateStrength(MyPlayerStats.StrengthFromItem, MyPlayerStats.DamageFromItem, MyPlayerStats.HealthFromItem);
        GeneralUpdate();
    }

    public void AddAgility()
    {
        PlayerStats.BaseAgility++;
        MyPlayerStats.UpdateAgility(MyPlayerStats.AgilityFromItem, MyPlayerStats.DefenseFromItem, MyPlayerStats.AttackSpeedFromItem);
        GeneralUpdate();
    }

    public void AddIntelligence()
    {
        PlayerStats.BaseIntelligence++;
        MyPlayerStats.UpdateIntelligence(MyPlayerStats.IntelligenceFromItem, MyPlayerStats.SpellPowerFromItem, MyPlayerStats.ManaFromItem);
        GeneralUpdate();
    }
}
