using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public int GameOverCase { get; set; }
    [SerializeField] Text GameOverText;

    public void GameOverDisplayEvent()
    {
        if (GameOverCase == 0) //When the player is dead
        {
            GameOverText.text = "YOU'RE DEAD. TRY AGAIN!";
            SoundController.Instance.PlayGameOver();
        }

        else //When finishing the game
        {
            GameOverText.text = "CONGRATULATIONS! YOU'VE FINISHED THE GAME FOR NOW.";
            SoundController.Instance.PlayVictory();
        } 
        GameOverText.gameObject.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void ReturnToMainMenu()
    {
        UIController.ResetUISettings();
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
