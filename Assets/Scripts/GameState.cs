using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public static int state;
    public static Vector2 prevMousePosition;
    public static Vector2 cursorVelocity;


    public const int START = 0;
    public const int GAMEPLAY = 1;
    public const int GAMEOVER = 2;

    public GameObject gameOverUI;
    public GameObject tutorialUI;
    public Text HighScoreText;

    public PlayerMovement player;
    public float gameOverAnimationDuration;

    public Animator welcomeScreenAnimator;
    public Animator tutorialAnimator;

    public Toggle audioToggle;

    float gameOverAnimationTimer;

    void Start()
    {
        state = START;
        prevMousePosition = Input.mousePosition;
        audioToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("DisableAudio"));
        HighScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore");

        gameOverAnimationTimer = 0f;
    }

    void Update()
    {
        DecrementTimers();
    }

    private void LateUpdate()
    {
        cursorVelocity = ((Vector2) Input.mousePosition - prevMousePosition) / Time.deltaTime;

        // previous mouse position should be recorded at the end of the frame
        prevMousePosition = Input.mousePosition;
    }

    public void StartGame()
    {
        state = GAMEPLAY;
        player.StartPlayer();

        welcomeScreenAnimator.SetBool("gameHasStarted", true);

        if (!TutorialManager.tutorialOff)
            tutorialUI.SetActive(true);
    }

    public void EndGame()
    {
        state = GAMEOVER;

        // update high score
        int score = PlatformManager.numObstaclesPassed;
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            gameOverUI.GetComponent<GameOverDisplay>().DeclareNewRecord();
        }

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
}
