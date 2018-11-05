using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTrap : MonoBehaviour
{
    int CollisionCount = 0;
    int TriggerCount = 0;

    Animator MissileTrapAnim;
    Rigidbody2D MissileTrapRigid;
    CircleCollider2D MissileTrapCircle;

    [SerializeField] GameObject BeePrefab;

    int ActivateHarsh = Animator.StringToHash("Activate");

    //Use this for internal initializations
    void Awake()
    {
        MissileTrapAnim = GetComponent<Animator>();
        MissileTrapRigid = GetComponent<Rigidbody2D>();
        MissileTrapCircle = GetComponent<CircleCollider2D>();
    }

    void SpawningEvent()
    {
        for (int i = 0; i < 5; i++)
            Instantiate(BeePrefab, transform.position, Quaternion.identity);                 
    }

    void InactiveEvent()
    {
        MissileTrapCircle.isTrigger = true;
        MissileTrapRigid.velocity = new Vector2(0f, 0f);
        MissileTrapRigid.isKinematic = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionCount++;
        if (CollisionCount == 1)
            MissileTrapRigid.velocity = new Vector2(2f, 5f);
        else if (CollisionCount == 2)
            MissileTrapAnim.SetTrigger(ActivateHarsh);                             
    }

    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (TriggerCount == 0 && OtherObject.gameObject.layer == 8)  //Trigger only once with the Player layer
        {
            MissileTrapRigid.isKinematic = false;
            TriggerCount++;
        }           
    }
}
