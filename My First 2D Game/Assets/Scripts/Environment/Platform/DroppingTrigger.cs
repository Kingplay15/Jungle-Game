using UnityEngine;

public class DroppingTrigger : MonoBehaviour
{
    [SerializeField] DroppingPlatform MyPlatform;

    void OnCollisionEnter2D(Collision2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8 && !MyPlatform.AlreadyDrop) //Trigger the drop on collision with the player
            MyPlatform.Drop();
    }
}
