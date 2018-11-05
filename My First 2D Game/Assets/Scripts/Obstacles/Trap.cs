using UnityEngine;

public class Trap : MonoBehaviour
{
    public bool Activated { get; private set; } = false;

    Animator TrapAnim;

    [SerializeField] int Damage;
    public int GetDamage //CollisionDamage's getter
    {
        get { return Damage; }
    }

    int ActivateHarsh = Animator.StringToHash("Activate");

    //Use this for internal initializations
    void Awake()
    {
        TrapAnim = GetComponent<Animator>();
    }

    void ActivatedEvent()
    {
        Activated = true;
    }

    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8 && 
            OtherObject.gameObject.tag != "Attack")   //Cannot be activated by the sword
            TrapAnim.SetTrigger(ActivateHarsh);
    }
}
