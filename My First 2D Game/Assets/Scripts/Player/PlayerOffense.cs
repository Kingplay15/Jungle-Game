using UnityEngine;

public class PlayerOffense : MonoBehaviour
{
    //Player's offensive operations
    public int DefaultWeaponNum { get; private set; } = 3; //The default weapon is an iron sword

    public bool IsAttacking { get; private set; } = false;
    public int AttackNum { get; private set; }
    public int CurrentMana { get; set; }

    Animator PlayerAnim;
    AnimatorStateInfo PlayerStateInfo;
    PlayerMovement MovementComponent;
    PlayerDefense DefenseComponent;

    int AttackStateHarsh = Animator.StringToHash("Base Layer.Knight_Attack");
    int JummpingAttackStateHarsh = Animator.StringToHash("Base Layer.Knight_Jumping_Attack");
    int AttackHarsh = Animator.StringToHash("Attack");
    int AttackSpeedHarsh = Animator.StringToHash("AttackSpeed");

    void Awake()
    {
        PlayerAnim = GetComponent<Animator>();
        MovementComponent = GetComponent<PlayerMovement>();
        DefenseComponent = GetComponent<PlayerDefense>();
    }

    void Update()
    {
        PlayerStateInfo = PlayerAnim.GetCurrentAnimatorStateInfo(0);
        if (PlayerStateInfo.fullPathHash == AttackStateHarsh || PlayerStateInfo.fullPathHash == JummpingAttackStateHarsh)
            return;

        //Cannot control the player after death, while pausing or looking down
        if (!DefenseComponent.Dead && Time.timeScale != 0f && !MovementComponent.LookingDown)
            Attack();
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            float ConvertedSpeed = PlayerStats.TotalAttackSpeed / 100f;
            PlayerAnim.SetFloat(AttackSpeedHarsh, ConvertedSpeed);
            PlayerAnim.SetTrigger(AttackHarsh);
        }
    }

    void IsAttackingEvent()
    {
        AttackNum++;
        IsAttacking = true;
    }

    void NotAttackingEvent()
    {
        IsAttacking = false;
    }

}
