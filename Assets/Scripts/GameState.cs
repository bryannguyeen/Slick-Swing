using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static string state;
    public static bool canBoost;

    public GameObject gameOverUI;

    public PlayerMovement player;
    public float gameOverAnimationDuration;

    public Animator welcomeScreenAnimator;

    float gameOverAnimationTimer;

    void Start()
    {
        state = "start";
        canBoost = true;
        gameOverAnimationTimer = 0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (string.Equals(state, "game over") & gameOverAnimationTimer == 0f)
                ResetGame();

            if (string.Equals(state, "start"))
                StartGame();
        }

        decrementTimers();
    }

    public void StartGame()
    {
        state = "gameplay";
        player.StartPlayer();

        welcomeScreenAnimator.SetBool("gameHasStarted", true);
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
