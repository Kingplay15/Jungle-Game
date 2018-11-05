using UnityEngine;

public class Bee : Enemy
{
    //Motion
    [SerializeField] float UpDown;
    float VelocityY;

    //Life time
    float BornTime;
    float LifeTime;

    [SerializeField] GameObject BlueCircle;

    //Use this for internal initializations
    protected override void Awake()
    {
        EnemyRigid = GetComponent<Rigidbody2D>();
        OnDeathComponent = GetComponent<EnemyOnDeath>();
        Speed = Random.Range(1f, 2.5f);
        InvokeRepeating("AdjustMotion", 0.1f, 0.2f);
        BornTime = Time.time;

        PlayerXPInfo = FindObjectOfType<PlayerExperience>();
        PlayerOffenseInfo = FindObjectOfType<PlayerOffense>();
        PlayerDefenseInfo = FindObjectOfType<PlayerDefense>();
    }

    //Use this for everything except constant forces
    protected override void Update()
    {
        PlayerDetecing();

        if (Distance.x * transform.localScale.x < 0)
            ChangeFacingDirection();

        //Destroyed after 10 seconds
        LifeTime = Time.time - BornTime;
        if (LifeTime > 10f)
            Die();
    }

    void AdjustMotion()
    {
        UpDown *= -1f;
        if (Mathf.Abs(Distance.y) > 3f)
            VelocityY = Random.Range(1f, 5f) * 2f;
        else VelocityY = Random.Range(1f, 5f);
    }

    protected override void Move()
    {
        if (PlayerDefenseInfo.Dead)
            return;

        if (Distance.x > 0)
            EnemyRigid.velocity = new Vector2(Speed, Speed * UpDown * VelocityY);
        else if (Distance.x < 0)
            EnemyRigid.velocity = new Vector2(-Speed, Speed * UpDown * VelocityY);
    }

    void Die()
    {
        if (PlayerExperience.CurrentLevel < 10)
            PlayerXPInfo.UpdateXP(OnDeathComponent.XPReward);
        OnDeathComponent.DropItem();
        Instantiate(BlueCircle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8) //Player layer
        {
            if (OtherObject.gameObject.tag == "Knight" || OtherObject.gameObject.tag == "Attack" &&
                PlayerOffenseInfo.IsAttacking)
            {
                Die();
            }
        }
    }
}
