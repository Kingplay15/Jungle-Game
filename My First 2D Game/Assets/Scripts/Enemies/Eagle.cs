using UnityEngine;

public class Eagle : Enemy
{
    [SerializeField] Missile AttackPrefab;
    int AttackCount = 0;
    float TurmTimePoint = 0f;

    protected override void Update()
    {
        PlayerDetecing();
        CheckAttackingStatus();

        //Making the Enemy attacks the player from a certain distance
        if (ReadyToAttack)
        {
            Attack();
            AttackCount++;
            ReadyToAttack = false;
        }
    }

    protected override void Move()
    {
        //Adjusting the Enemy's moving direction
        if (transform.localScale.x > 0)
            EnemyRigid.velocity = new Vector2(Speed, EnemyRigid.velocity.y);
        else EnemyRigid.velocity = new Vector2(-Speed, EnemyRigid.velocity.y);
    }

    void SpawningEvent()
    {
        Missile[] FallingStones = new Missile[3];
        for (int i = 0; i < 3; i++)
        {
            FallingStones[i] = Instantiate(AttackPrefab, transform.GetChild(0).position, Quaternion.identity);
            FallingStones[i].GetSubjectScale = transform.localScale.x;
        }
    }

    protected override void CheckAttackingStatus()
    {
        if (Mathf.Abs(Distance.x) <= AttackRange.x && Distance.y < 0 && Distance.y >= -AttackRange.y &&
            Time.time > NextAttack && !PlayerDefenseInfo.Dead)
            ReadyToAttack = true;
    }

    protected override void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (Time.time - TurmTimePoint >= 1f && OtherObject.gameObject.layer == 10) //BackGround layer    
        {
            ChangeFacingDirection();
            TurmTimePoint = Time.time;
        }
    }

    void OnBecameInvisible()
    {
        if (AttackCount > 10)
            gameObject.SetActive(false);
    }
}
