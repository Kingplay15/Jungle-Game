using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] float Speed;

    [SerializeField] bool MovingHorizontal = false;
    bool MovingRight = false;
    bool MovingDown = false;

    PlayerMovement PlayerInfo;
    [SerializeField] BottomCollisionDetection BottomCollisionDetector;
    [SerializeField] TopCollisionDetection TopCollisionDetector;

    Rigidbody2D PlatformRigid;

    void Awake()
    {
        PlatformRigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        PlayerInfo = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        //Turning around to not press the player towards the wall
        AvoidPlayer();
    }

    void FixedUpdate()
    {
        if (Speed > 0f)
            Move();
    }

    void Move()
    {
        if (MovingHorizontal)
        {
            if (MovingRight)
                PlatformRigid.velocity = new Vector2(Speed, PlatformRigid.velocity.y);
            else //Moving left
                PlatformRigid.velocity = new Vector2(-Speed, PlatformRigid.velocity.y);
        }

        else //Moving vertically
        {
            if (MovingDown)
                PlatformRigid.velocity = new Vector2(PlatformRigid.velocity.x, -Speed);
            else //Moving up
                PlatformRigid.velocity = new Vector2(PlatformRigid.velocity.x, Speed);
        }
    }

    void Turn()
    {
        MovingDown = !MovingDown;
        MovingRight = !MovingRight;
    }

    void AvoidPlayer()
    {
        //The player is beneath the platform
        if (BottomCollisionDetector.PlayerInRange && PlayerInfo.BackgroundCollisionCount > 0)
        {
            Turn();
            BottomCollisionDetector.PlayerInRange = false;
        }

        //The player is on the platform
        else if (TopCollisionDetector.PlayerInRange && PlayerInfo.BackgroundCollisionCount > 0)
        {
            Turn();
            TopCollisionDetector.PlayerInRange = false;
        }
    }

    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 10)  //Background layer
            Turn();
    }
}
