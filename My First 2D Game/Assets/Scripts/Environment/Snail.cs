using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : MonoBehaviour
{
    [SerializeField] float Speed;
    bool IsTurning = false;

    Animator SnailAnim;
    Rigidbody2D SnailRigid;

    int TurnHarsh = Animator.StringToHash("Turn");

    // Use this for internal initializations
    void Awake()
    {
        SnailRigid = GetComponent<Rigidbody2D>();
        SnailAnim = GetComponent<Animator>();
    }

    //Use this for applying constant forces
    void FixedUpdate()
    {
        if (!IsTurning)
            Move();
    }

    void Move()
    {
        //Adjusting the snail's moving direction
        if (transform.localScale.x > 0)
            SnailRigid.velocity = new Vector2(-Speed, SnailRigid.velocity.y);
        else if (transform.localScale.x < 0)
            SnailRigid.velocity = new Vector2(Speed, SnailRigid.velocity.y);
    }

    void ChangeFacingDirection()
    {
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    void Turn()
    {
        SnailAnim.SetTrigger(TurnHarsh);
        IsTurning = true;
    }

    void NotTurningEvent()
    {
        IsTurning = false;
        ChangeFacingDirection();
    }

    void OnCollisionEnter2D(Collision2D OtherObject)
    {
        if (OtherObject.gameObject.tag == "Edge" || OtherObject.gameObject.tag == "Wall")
            Turn();
    }
}
