using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static int state;
    public static Vector2 prevMousePosition;


    public const int START = 0;
    public const int GAMEPLAY = 1;
    public const int GAMEOVER = 2;

    public GameObject gameOverUI;

    public PlayerMovement player;
    public float gameOverAnimationDuration;

    public Animator welcomeScreenAnimator;

    float gameOverAnimationTimer;

    void Start()
    {
        state = START;
        prevMousePosition = Input.mousePosition;

        gameOverAnimationTimer = 0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (state == GAMEOVER & gameOverAnimationTimer == 0f)
                ResetGame();

            if (state == START)
                StartGame();
        }

        decrementTimers();
    }

    private void LateUpdate()
    {
        // previous mouse position should be recorded at the end of the frame
        prevMousePosition = Input.mousePosition;
    }

    public void StartGame()
    {
        state = GAMEPLAY;
        player.StartPlayer();

        welcomeScreenAnimator.SetBool("gameHasStarted", true);
    }

    public void EndGame()
    {
        state = GAMEOVER;

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
