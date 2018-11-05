using UnityEngine;

public class Missile : MonoBehaviour
{
    //Motion
    float Vector2XCompo, Vector2YCompo;
    [SerializeField] float XMin;
    [SerializeField] float XMax;
    [SerializeField] float YMin;
    [SerializeField] float YMax;
    [SerializeField] float Speed;
    float SubjectScale; //Scale of the subject that fired this missile
    public float GetSubjectScale //Setter
    {
        set{ SubjectScale = value; }
    }

    //Life time
    float BornTime;
    float LifeTime;

    Rigidbody2D MissileRigid;

    // Use this for internal initializations
    void Awake()
    {
        MissileRigid = GetComponent<Rigidbody2D>();
        Vector2XCompo = Random.Range(XMin, XMax);
        Vector2YCompo = Random.Range(YMin, YMax);
        BornTime = Time.time;
    }

    // Use this for external initializations
    void Start()
    {
        //Making the attack's flying direction following the attacker's facing direction
        if (SubjectScale > 0)
            MissileRigid.AddForce(new Vector2(Vector2XCompo, Vector2YCompo) * Speed);
        else MissileRigid.AddForce(new Vector2(-Vector2XCompo, Vector2YCompo) * Speed);
    }

    //Use this for everything except constant forces
    void Update()
    {
        //Destroyed after 5 seconds
        LifeTime = Time.time - BornTime;
        if (LifeTime > 5f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        //Destroyed on a collision with the player's body or the ground
        if (OtherObject.gameObject.layer == 8 && OtherObject.gameObject.tag != "Attack" //Not colliding with the sword
            || OtherObject.gameObject.layer == 10 && OtherObject.gameObject.tag != "Border")  //Not colliding with borders
            Destroy(gameObject);
    }
}
