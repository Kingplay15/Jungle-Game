using UnityEngine;

public class PlayerDefense : MonoBehaviour
{
    UIController GameUIController;

    Rigidbody2D PlayerRigid;
    Animator PlayerAnim;

    [SerializeField] Vector2 BounceLeft;
    [SerializeField] Vector2 BounceRight;

    //Player's defensive operations
    public int CurrentHealth { get; set; }
    int TakeDamage = 0;

    public int DefaultShieldNum { get; private set; } = 2; //The default shield is an iron shield

    public bool Dead { get; private set; } = false;

    //Enemies and Hazards
    [SerializeField] Enemy[] PlayerEnemies;
    [SerializeField] Trap[] PlayerTraps;
    [SerializeField] Hazard[] PlayerHazards;

    //Harsh IDs
    int GetHurtHarsh = Animator.StringToHash("GetHurt");
    int DeadHarsh = Animator.StringToHash("Dead");

    //Use this for internal initializations
    void Awake()
    {
        PlayerRigid = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponent<Animator>();
        GameUIController = FindObjectOfType<UIController>();
    }

    void GetHurt(GameObject OtherObject)
    {
        if (Dead) //Cannot getting hurt after death
            return;

        //Getting hurt               
        switch (OtherObject.gameObject.tag)
        {
            case "Attack": //Damage from the enemy's melee attack           
                foreach (Enemy element in PlayerEnemies)
                    //Taking damage only if the enemy is attacking
                    if (OtherObject.gameObject.transform.IsChildOf(element.transform) && !element.IsAttacking)
                        return;
                    else if (OtherObject.gameObject.transform.IsChildOf(element.transform) && element.IsAttacking)
                        TakeDamage = element.GetAttackDamge;
                break;

            case "Missile": //Damage from the enemy's range attack           
                foreach (Enemy element in PlayerEnemies)
                    if (OtherObject.transform.GetChild(0).gameObject.tag == element.gameObject.tag)
                        TakeDamage = element.GetAttackDamge;
                break;

            case "Body": //Damage from colliding with the enemy
                foreach (Enemy element in PlayerEnemies)
                    if (OtherObject.gameObject.transform.IsChildOf(element.transform))
                        TakeDamage = element.GetCollisionDamage;
                break;

            case "Bee": //Damage from the bee
                foreach (Enemy element in PlayerEnemies)
                    if (OtherObject.gameObject.tag == element.gameObject.tag)
                        TakeDamage = element.GetCollisionDamage;
                break;

            case "Trap": //Damage from the trap           
                foreach (Trap element in PlayerTraps)
                    if (OtherObject.gameObject.name == element.gameObject.name && element.Activated)
                        return;
                    else if (OtherObject.gameObject.name == element.gameObject.name && !element.Activated)
                        TakeDamage = element.GetDamage;
                break;

            case "Hazard": //Damage from the hazard
                foreach (Hazard element in PlayerHazards)
                    if (OtherObject.gameObject.tag == element.gameObject.tag)
                        TakeDamage = element.GetDamage;
                break;
        }

        //Getting hurt animation
        PlayerAnim.SetTrigger(GetHurtHarsh);

        //Taking damage
        int RoundedDeffense = PlayerStats.TotalDefense / 10;
        int RealDamageTaken = TakeDamage - RoundedDeffense;
        if (RealDamageTaken < 1) //Damage taken after couting deffense cannot be lower than 1
            RealDamageTaken = 1;
        CurrentHealth = CurrentHealth - RealDamageTaken;
        if (CurrentHealth < 0)
            CurrentHealth = 0;
        GameUIController.UpdateHealthBar();

        //Adjusting facing direction when getting hurt
        float Distance = OtherObject.gameObject.transform.position.x - transform.position.x;
        bool FacingRight;
        if (transform.localScale.x > 0)
            FacingRight = true;
        else FacingRight = false;
        switch (FacingRight)
        {
            case true:
                if (Distance < 0)
                {
                    Vector3 PlayerScale = transform.localScale;
                    PlayerScale.x *= -1;
                    transform.localScale = PlayerScale;
                    break;
                }
                else break;

            case false:
                if (Distance > 0)
                {
                    Vector3 PlayerScale = transform.localScale;
                    PlayerScale.x *= -1;
                    transform.localScale = PlayerScale;
                    break;
                }
                else break;
        }
        //Bouncing back after getting hurt
        if (Distance > 0)
            PlayerRigid.velocity = BounceLeft;
        else PlayerRigid.velocity = BounceRight;

        if (CurrentHealth == 0)
        {
            PlayerAnim.SetBool(DeadHarsh, true);
            Dead = true;
            SoundController.Instance.PlayPlayerDie(); //Death sound
        }
        //Getting hurt sound
        else SoundController.Instance.PlayPlayerGetHurt();
    }

    public void DieEvent()
    {
        UIController.Instance.DisplayGameOver(0);
    }

    void OnCollisionEnter2D(Collision2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 9 || OtherObject.gameObject.layer == 11) //Enemy and Obstacle layer
        {
            GetHurt(OtherObject.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 9 || OtherObject.gameObject.layer == 11) //Enemy and Obstacle layer
        {
            GetHurt(OtherObject.gameObject);
        }
    }
}
