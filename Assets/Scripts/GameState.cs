using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static string state = "start";
    public GameObject gameOverUI;
    public float gameOverAnimationDuration;

    float gameOverAnimationTimer;

    void Start()
    {
        state = "start";

        gameOverAnimationTimer = 0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (string.Equals(state, "game over") & gameOverAnimationTimer == 0f)
                ResetGame();
        }

        decrementTimers();
    }

    public void StartGame()
    {
        state = "gameplay";
    }

    public void EndGame()
    {
        state = "game over";

        // update high score
        int score = PlatformManager.numObstaclesPassed;
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            gameOverUI.GetComponent<GameOverDisplay>().declareNewRecord();
        }

        // display game over screen
        gameOverUI.SetActive(true);

        gameOverAnimationTimer = gameOverAnimationDuration;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void decrementTimers()
    {
        gameOverAnimationTimer -= Time.deltaTime;
        if (gameOverAnimationTimer < 0f)
            gameOverAnimationTimer = 0f;
    }
}
