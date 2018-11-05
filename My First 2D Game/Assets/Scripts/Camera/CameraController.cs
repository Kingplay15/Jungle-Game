using UnityEngine;

public class CameraController : MonoBehaviour
{
     Vector3 PosAfterSmoothDamp;
    public bool DoneSmoothDamp { get; private set; } = false;

    //View zone of the scene
    float MostLeft = 9f;
    float MostRight = 92f;
    float Bottom = -50.9f;
    float Top = -2.7f;

    PlayerMovement MainPlayer;
    Vector3 Offset = new Vector3(0f, 3f, -10f); //The ideal positon difference between the player and camera

    //For changing position when the player looks down
    Vector3 Target = new Vector3(0f, 0f, 0f);
    Vector3 Velocity = Vector3.zero;

    void Awake()
    {
        DoneSmoothDamp = false;
        PosAfterSmoothDamp = new Vector3(0f, 0f, 0f);
    }

    void Start()
    {
        if (SceneController.Instance.CurrentScene.name == "MainMenu")
            Target = new Vector3(0f, -10f, -10f);

        else MainPlayer = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (SceneController.Instance.CurrentScene.name == "MainMenu")
        {
            if (!DoneSmoothDamp)
                transform.position = Vector3.SmoothDamp(transform.position, Target, ref Velocity, 1f);
        }

        else MoveDown();
    }

    //Use this for something that should happen after update 
    void LateUpdate()
    {
        if (SceneController.Instance.CurrentScene.name == "MainMenu")
        {
            if (transform.position == PosAfterSmoothDamp)
            {
                if (!DoneSmoothDamp)
                    DoneSmoothDamp = true;
            }
            else PosAfterSmoothDamp = transform.position;
        }

        else if (SceneController.Instance.CurrentScene.name == "Level1")
        {
            Target = transform.position + new Vector3(0f, -5f, 0f);

            if (!MainPlayer.LookingDown)
            {
                transform.position = MainPlayer.transform.position + Offset;

                if (transform.position.x < MostLeft)
                {
                    if (transform.position.y > Top)
                        transform.position = new Vector3(MostLeft, Top, -10f);
                    else if (transform.position.y < Bottom)
                        transform.position = new Vector3(MostLeft, Bottom, -10f);
                    else transform.position = new Vector3(MostLeft, MainPlayer.transform.position.y + Offset.y, -10f);
                }

                else if (transform.position.x > MostRight)
                {
                    if (transform.position.y > Top)
                        transform.position = new Vector3(MostRight, Top, -10f);
                    else if (transform.position.y <= Bottom)
                        transform.position = new Vector3(MostRight, Bottom, -10f);
                    else transform.position = new Vector3(MostRight, MainPlayer.transform.position.y + Offset.y, -10f);
                }

                else
                {
                    if (transform.position.y > Top)
                        transform.position = new Vector3(MainPlayer.transform.position.x + Offset.x, Top, -10f);
                    else if (transform.position.y < Bottom)
                        transform.position = new Vector3(MainPlayer.transform.position.x + Offset.x, Bottom, -10f);
                }
            }
        }
    }

    void MoveDown()
    {
        if (MainPlayer.LookingDown && transform.position.y - MainPlayer.transform.position.y >= -2f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, Target, ref Velocity, 2f);
        }
    }
}
