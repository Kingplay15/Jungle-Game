using UnityEngine;

public class GemHolder : MonoBehaviour
{
    public int HolderNum;
    [SerializeField] GameObject GemDisplay;

    [SerializeField] GameObject MyStandingPlatform;
    [SerializeField] GameObject MyMovingWall;

    public bool PlayerInRange { get; private set; } = false;
    public bool AlreadyTriggered { get; private set; } = false;

    void OnTriggerEnter2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8) //Player layer
            PlayerInRange = true;
    }

    void OnTriggerExit2D(Collider2D OtherObject)
    {
        if (OtherObject.gameObject.layer == 8) //Player layer
            PlayerInRange = false;
    }

    public void TriggerEvent()
    {
        if (HolderNum == 0) //Blue Holder
            MyStandingPlatform.SetActive(true); //Make a platform appears when triggered
        else MyMovingWall.SetActive(false);

        GemDisplay.SetActive(true);
        AlreadyTriggered = true;
    }
}
