using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public void OnClick(string ButtonName)
    {
        switch (ButtonName)
        {
            case "NewGame":
                SceneManager.LoadSceneAsync("CreateCharacter", LoadSceneMode.Single);
                break;
            case "LoadCharacter":
                SceneManager.LoadSceneAsync("LoadCharacter", LoadSceneMode.Single);
                break;
            case "ConfirmLoading":
                SceneController.Instance.NewGameStarted = false;
                SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);
                break;
            case "DoneCreating":
                SceneController.Instance.NewGameStarted = true;
                SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);
                break;
            case "CancelCreating":
                SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
                break;
            case "Save":                
                SaveLoadController.Instance.Save();
                break;
            case "Exit":
                UIController.ResetUISettings();
                SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }
}
