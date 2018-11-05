using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    CameraController MainCamera;

    [SerializeField] GameObject GameName;
    [SerializeField] GameObject Options;
    [SerializeField] GameObject Credit;

    void Awake()
    {
        MainCamera = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        if(MainCamera.DoneSmoothDamp)
        {
            GameName.SetActive(true);
            Options.SetActive(true);
            Credit.SetActive(true);
        }
    }
}
