using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public PlayerState playerState;
    public static int state;
    public static Vector2 prevMousePosition;
    public static Vector2 cursorVelocity;

    public const int START = 0;
    public const int GAMEPLAY = 1;
    public const int GAMEOVER = 2;

    public GameObject gameOverUI;
    public GameObject welcomeUI;

    public Text HighScoreText;

    public PlayerMovement player;
    public float gameOverAnimationDuration;

    public Animator welcomeScreenAnimator;
    public Animator tutorialAnimator;

    float gameOverAnimationTimer;

    void Start()
    {
        state = START;
        prevMousePosition = Input.mousePosition;
        HighScoreText.text = "HIGH SCORE: " + GetHighScore();

        gameOverAnimationTimer = 0f;
    }

    void Update()
    {
        cursorVelocity = ((Vector2)Input.mousePosition - prevMousePosition) / Time.deltaTime;

        DecrementTimers();
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

        welcomeScreenAnimator.SetTrigger("disappear");

        if (!TutorialManager.tutorialOff)
            tutorialAnimator.SetTrigger("showTutorial");
    }

    public void EndGame()
    {
        state = GAMEOVER;
        playerState.KillPlayer();

        // update high score
        int score = PlatformManager.numObstaclesPassed;
        if (score > GetHighScore())
        {
            SetHighScore(score);
            gameOverUI.GetComponent<GameOverDisplay>().DeclareNewRecord();
        }

        // turn off welcome screen ui
        welcomeUI.SetActive(false);

        // display game over screen
        gameOverUI.SetActive(true);

        // turn of tutorial ui if it is currently on
        tutorialAnimator.SetBool("firstSwing", true);

        gameOverAnimationTimer = gameOverAnimationDuration;
    }

    public void ResetGame()
    {
        if (gameOverAnimationTimer == 0f)
            SceneManager.LoadScene("Main");
    }

    public void DecrementTimers()
    {
        gameOverAnimationTimer -= Time.deltaTime;
        if (gameOverAnimationTimer < 0f)
            gameOverAnimationTimer = 0f;
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    public static void SetHighScore(int highScore)
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }
}
