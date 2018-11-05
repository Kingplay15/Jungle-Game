using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance = null;

    public bool NewGameStarted { get; set; } = false;

    public Scene CurrentScene { get; private set; }

    //Mouse icon
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void Awake()
    {
        //There has to be only ONE instance of SceneController in the game
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);

        CurrentScene = SceneManager.GetActiveScene();

        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CurrentScene = SceneManager.GetActiveScene();
        if (CurrentScene.name == "MainMenu")
        {
            SoundController.Instance.FindBGSoundTrack();
            SoundController.Instance.PlayMainMenu();
        }            
        else if (CurrentScene.name == "Level1")
        {
            Cursor.visible = false;
            if (!NewGameStarted)
                SaveLoadController.Instance.LoadGame();
        }
        else if (CurrentScene.name == "LoadCharacter")       
            SaveLoadController.Instance.CheckSaveFile();       
    }
}
