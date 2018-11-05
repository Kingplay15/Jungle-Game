using UnityEngine;

public class BottomCollisionDetection : MonoBehaviour
{
    public bool PlayerInRange { get; set; }

    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8 && OtherObject.gameObject.tag != "Attack")   //Player's body
            PlayerInRange = true;
    }

    void OnTriggerExit2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8 && OtherObject.gameObject.tag != "Attack")  //Player's body
            PlayerInRange = false;
    }
}
