using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    Inventory PlayerInventory;

    [SerializeField] StatPointAssignment[] GameStatsPointAssigment;
    [SerializeField] Text CurrentLvlDisplay;
    [SerializeField] Text LvlUpInformer;
    Animator InformerAnim;
    int DisplayHarsh = Animator.StringToHash("Display");
    public static int CurrentLevel { get; set; }

    [SerializeField] Text CurrentXPDisplay;
    [SerializeField] Slider CurrentXPFill;
    public static int CurrentXP { get; set; }
    float[] NextLevelXPFloat = new float[10]; //Max level is 10
    int[] NextLevelXP = new int[10]; //Convert next level XP to int value

    void Awake()
    {
        PlayerInventory = FindObjectOfType<Inventory>();

        NextLevelXPFloat[0] = 20f; //First level up requires 20 XPs
        NextLevelXP[0] = (int)NextLevelXPFloat[0];
        for (int i = 1; i < 10; i++)
        {
            NextLevelXPFloat[i] = NextLevelXPFloat[i - 1] * 1.5f;
            NextLevelXP[i] = (int)NextLevelXPFloat[i];
        }

        InformerAnim = LvlUpInformer.gameObject.GetComponentInParent<Animator>();
    }

    void Start()
    {
        //Starting level and XP of the player
        if (SceneController.Instance.NewGameStarted)
        {
            CurrentLevel = 1;
            CurrentXP = 0;
        }
        UpdateUIs();
    }

    public void UpdateXP(int Value)
    {
        CurrentXP += Value;
        while (CurrentXP >= NextLevelXP[CurrentLevel - 1])
        {
            CurrentXP -= NextLevelXP[CurrentLevel - 1];
            LevelUp();
        }
        UpdateUIs();
    }

    void LevelUp()
    {
        CurrentLevel++;
        PlayerInventory.LoopItemRequirements();
        LvlUpInformer.text = "LEVEL UP";
        InformerAnim.SetTrigger(DisplayHarsh);
        CurrentLvlDisplay.text = "LEVEL " + CurrentLevel.ToString();
        StatPointAssignment.StatPoints += 3;
        for (int i = 0; i < 3; i++)
            GameStatsPointAssigment[i].UpdateStatPoints();
        SoundController.Instance.PlayLevelUp();
    }

    void UpdateUIs()
    {
        if (CurrentLevel == 10) //Max level already
            CurrentLvlDisplay.text = "LEVEL " + CurrentLevel.ToString() + " (MAX)";
        else CurrentLvlDisplay.text = "LEVEL " + CurrentLevel.ToString();
        CurrentXPDisplay.text = CurrentXP + "/" + NextLevelXP[CurrentLevel - 1];
        CurrentXPFill.maxValue = NextLevelXP[CurrentLevel - 1];
        CurrentXPFill.value = CurrentXP;
    }
}
