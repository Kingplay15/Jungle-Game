using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Fields for the enemy's motion
    [SerializeField] protected float Speed;
    [SerializeField] Vector2 DetectionRange;
    bool PlayerDetected = false;
    protected Vector3 Distance;
    bool StopChasing = false;

    //Fields for the enemy's offensive operations
    [SerializeField] protected float AttackDelay;
    [SerializeField] protected Vector2 AttackRange;

    [SerializeField] int CollisionDamage;
    public int GetCollisionDamage //CollisionDamage's getter
    {
        get { return CollisionDamage; }
    }

    [SerializeField] int AttackDamage;
    public int GetAttackDamge //AttackDamage's getter
    {
        get { return AttackDamage; }
    }

    protected float NextAttack = 0f;
    public bool IsAttacking { get; private set; } = false;
    protected bool ReadyToAttack = false;

    //Fields for the enemy's defensive operations
    [SerializeField] float Health;
    int TakeDamage = 0;
    int TakeDamageNum = 0;
    bool Dead = false;

    //Other fields
    protected PlayerExperience PlayerXPInfo;
    protected PlayerOffense PlayerOffenseInfo;
    protected PlayerDefense PlayerDefenseInfo;
    protected Rigidbody2D EnemyRigid;
    protected Animator EnemyAnim;
    protected EnemyOnDeath OnDeathComponent;

    //Harsh IDs
    int MovingHarsh = Animator.StringToHash("Moving");
    protected int AttackHarsh = Animator.StringToHash("Attack");
    int GetHurtHarsh = Animator.StringToHash("GetHurt");
    protected int DieHarsh = Animator.StringToHash("Die");

    //Use this for internal initializations
    protected virtual void Awake()
    {
        EnemyRigid = GetComponent<Rigidbody2D>();
        EnemyAnim = GetComponent<Animator>();
        OnDeathComponent = GetComponent<EnemyOnDeath>();

        PlayerXPInfo = FindObjectOfType<PlayerExperience>();
        PlayerOffenseInfo = FindObjectOfType<PlayerOffense>();
        PlayerDefenseInfo = FindObjectOfType<PlayerDefense>(); 
    }

    //Use this for external initializations
    void Start()
    {
        Distance = PlayerOffenseInfo.transform.position - transform.position;
    }

    //Use this for everything except constant forces
    protected virtual void Update()
    {
        PlayerDetecing();
        CheckAttackingStatus();

        if (Speed > 0 && !PlayerDetected)
            EnemyAnim.SetBool(MovingHarsh, false);

        if (Distance.x * transform.localScale.x < 0 && PlayerDetected)
        {
            StopChasing = false;
            ChangeFacingDirection();
        }

        //Making the Enemy attacks the Player from a certain distance
        if (ReadyToAttack)
        {
            Attack();
            ReadyToAttack = false;
        }
    }

    //Use this for apPlying constant forces
    void FixedUpdate()
    {
        //if the enemy is abled to move and is chasing the Player
        if (Speed > 0 && PlayerDetected && !Dead) 
            Move();
    }

    //Use this for apPlying something that has to be done after Update
    void LateUpdate()
    {
        Distance = PlayerOffenseInfo.transform.position - transform.position;
    }

    protected virtual void Move()
    {
        //Moving the Enemy to a certain distance from the Player
        if (Mathf.Abs(Distance.x) > AttackRange.x && !StopChasing)
        {
            EnemyAnim.SetBool(MovingHarsh, true);

            //Adjusting the Enemy's moving direction
            if (Distance.x > 0)
                EnemyRigid.velocity = new Vector2(Speed, EnemyRigid.velocity.y);
            else if (Distance.x < 0)
                EnemyRigid.velocity = new Vector2(-Speed, EnemyRigid.velocity.y);
        }
        else if (Mathf.Abs(Distance.x) <= AttackRange.x || StopChasing)
            EnemyAnim.SetBool(MovingHarsh, false);
    }

    protected void PlayerDetecing()
    {
        if (Mathf.Abs(Distance.x) < DetectionRange.x && Mathf.Abs(Distance.y) < DetectionRange.y)
            PlayerDetected = true;

        else if (Mathf.Abs(Distance.x) > DetectionRange.x || Mathf.Abs(Distance.y) > DetectionRange.y)
            PlayerDetected = false;
    }

    protected void ChangeFacingDirection()
    {
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    protected void Attack()
    {
        NextAttack = Time.time + AttackDelay;
        EnemyAnim.SetTrigger(AttackHarsh);
    }

    protected virtual void CheckAttackingStatus()
    {
        if (Mathf.Abs(Distance.x) <= AttackRange.x && Mathf.Abs(Distance.y) <= AttackRange.y &&
            Time.time > NextAttack && !PlayerDefenseInfo.Dead)
            ReadyToAttack = true;
    }

    void IsAttackingEvent()
    {
        IsAttacking = true;
    }

    void NotAttackingEvent()
    {
        IsAttacking = false;
    }

    void GetHurt(Collider2D OtherObject)
    {
        if (Dead || TakeDamageNum == PlayerOffenseInfo.AttackNum) 
            return;

        //Getting hurt animation
        EnemyAnim.SetTrigger(GetHurtHarsh);

        //Taking damage
        TakeDamageNum = PlayerOffenseInfo.AttackNum;
        TakeDamage = PlayerStats.TotalDamage;
        Health -= TakeDamage;
    }

    void DieEvent()
    {
        if (Health <= 0f)
        {
            EnemyAnim.SetTrigger(DieHarsh);
            Dead = true;
        }           
    }

    void OnCollisionEnter2D(Collision2D OtherObject)
    {
        if (OtherObject.gameObject.tag == "Edge" || OtherObject.gameObject.tag == "Wall")
            StopChasing = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8) //Player layer
            if (OtherObject.gameObject.tag == "Attack" && PlayerOffenseInfo.IsAttacking)
                GetHurt(OtherObject);
    }

    //Animation event function
    protected void DisappearEvent()
    {
        if (PlayerExperience.CurrentLevel < 10)
            PlayerXPInfo.UpdateXP(OnDeathComponent.XPReward);
        OnDeathComponent.DropItem();
        gameObject.SetActive(false);
    }   
}
