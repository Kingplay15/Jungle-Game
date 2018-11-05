using UnityEngine;

public class TopCollisionDetection : BottomCollisionDetection
{
    PlayerMovement Knight;

    void Awake()
    {
        Knight = FindObjectOfType<PlayerMovement>();
    }

    void OnTriggerStay2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8 && OtherObject.gameObject.tag != "Attack" &&
            Knight.BackgroundCollisionCount == 0)   //Player's body
            PlayerInRange = true;
    }
}
