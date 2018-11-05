using UnityEngine;

public class EndGameBorder : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8 && OtherObject.gameObject.tag != "Attack")  //Player layer
            UIController.Instance.DisplayGameOver(1);
    }
}
