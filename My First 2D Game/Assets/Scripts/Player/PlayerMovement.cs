using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player's movement
    [SerializeField] float Speed;
    [SerializeField] float JumpingHeight;
    float Direction = 0f;
    Transform Destination; //When teleporting
    public bool Teleporting { get; private set; } = false;
    public int BackgroundCollisionCount { get; private set; } = 0;
    int OnGroundCount;
    public bool LookingDown { get; private set; } = false;

    [SerializeField] Door[] PlayerDoors;

    //Components
    AnimatorStateInfo PlayerStateInfo;
    Animator PlayerAnim;
    Rigidbody2D PlayerRigid;
    BoxCollider2D PlayerBox;
    PlayerOffense OffenseComponent;
    PlayerDefense DefenseComponent;

    //Harsh IDs
    int SpeedHarsh = Animator.StringToHash("Speed");
    int JumpHarsh = Animator.StringToHash("Jump");
    int GoInsideHarsh = Animator.StringToHash("GoInside");
    int OnGroundHarsh = Animator.StringToHash("OnGround");
    int LookingDownHarsh = Animator.StringToHash("LookingDown");

    int[] MovingStateHarsh = new int[]
    {
            Animator.StringToHash("Base Layer.Knight_Walk"),
            Animator.StringToHash("Base Layer.Knight_Jump")
    };

    //Use this for internal initializations
    void Awake()
    {
        PlayerAnim = GetComponent<Animator>();
        PlayerRigid = GetComponent<Rigidbody2D>();
        PlayerBox = GetComponent<BoxCollider2D>();
        OffenseComponent = GetComponent<PlayerOffense>();
        DefenseComponent = GetComponent<PlayerDefense>();
    }

    //Use this for everything except constant forces
    void Update()
    {
        if (!DefenseComponent.Dead && Time.timeScale != 0f)  //Cannot control the player after death, while pausing...
        {
            PlayerStateInfo = PlayerAnim.GetCurrentAnimatorStateInfo(0);

            //Getting input for moving                     
            Direction = Input.GetAxis("Horizontal");

            if (!LookingDown && !OffenseComponent.IsAttacking)  //...or while looking down, attacking
            {
                ChangeFacingDirection();
                Jump();
            }
            LookDown();
            StopLookingDown();
        }
    }

    //Use this for applying constant forces
    void FixedUpdate()
    {
        if (!Teleporting)
            Move();
    }

    void Move()
    {
        PlayerAnim.SetFloat(SpeedHarsh, Mathf.Abs(Direction));

        //Only allowing moving while waking and jumping
        foreach (int element in MovingStateHarsh)
            if (PlayerStateInfo.fullPathHash == element)
                PlayerRigid.velocity = new Vector2(Direction * Speed, PlayerRigid.velocity.y);
    }

    void GoInsideEvent()
    {
        Teleporting = true;
        PlayerBox.isTrigger = true;
        PlayerRigid.drag = 100f;
        PlayerRigid.gravityScale = 0f;
    }

    void GoOutsideEvent()
    {
        Teleporting = false;
        PlayerBox.isTrigger = false;
        PlayerRigid.drag = 0f;
        PlayerRigid.gravityScale = 3f;
    }

    void TeleportEvent()
    {
        PlayerRigid.MovePosition(new Vector2(Destination.position.x, Destination.position.y - 1f));
        OnGroundCount--;
        if (OnGroundCount == 0)
            PlayerAnim.SetBool(OnGroundHarsh, false);
        BackgroundCollisionCount--;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && OnGroundCount > 0)
            PlayerAnim.SetTrigger(JumpHarsh);
    }

    void JumpEvent()
    {
        PlayerRigid.velocity = new Vector2(PlayerRigid.velocity.x, JumpingHeight);
    }

    void ChangeFacingDirection()
    {
        Vector3 PlayerScale = transform.localScale;
        if (Direction * PlayerScale.x < 0)
            PlayerScale.x *= -1;
        transform.localScale = PlayerScale;
    }

    void LookDown()
    {
        if (Input.GetKey(KeyCode.S))
        {
            PlayerAnim.SetBool(LookingDownHarsh, true);
            LookingDown = true;
        }
    }

    void StopLookingDown()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            PlayerAnim.SetBool(LookingDownHarsh, false);
            LookingDown = false;
        }
    }

    void OnCollisionEnter2D(Collision2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 10) //BackGround layer
            BackgroundCollisionCount++;

        if (OtherObject.gameObject.tag == "Ground" || OtherObject.gameObject.tag == "Edge" ||
            OtherObject.gameObject.tag == "Hazard")
        {
            OnGroundCount++;
            PlayerAnim.SetBool(OnGroundHarsh, true);
        }
    }

    void OnCollisionExit2D(Collision2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 10) //BackGround layer
            BackgroundCollisionCount--;

        if (OtherObject.gameObject.tag == "Ground" || OtherObject.gameObject.tag == "Edge" ||
            OtherObject.gameObject.tag == "Hazard")
        {
            OnGroundCount--;
            if (OnGroundCount == 0)
                PlayerAnim.SetBool(OnGroundHarsh, false);
        }
    }

    void OnTriggerStay2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.tag == "Door" && Input.GetKeyDown(KeyCode.W))
        {
            PlayerAnim.SetTrigger(GoInsideHarsh);
            foreach (Door element in PlayerDoors)
                if (OtherObject.gameObject.name == element.gameObject.name)
                    Destination = element.GetPosition;
        }
    }
}
