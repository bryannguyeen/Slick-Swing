using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float gameOverAnimationDuration;

    public Animator tutorialAnimator;
    public Animator transitionAnimator;

    bool enableResetButton;

    void Start()
    {
        state = START;
        prevMousePosition = Input.mousePosition;

        enableResetButton = false;
    }

    void Update()
    {
        cursorVelocity = ((Vector2)Input.mousePosition - prevMousePosition) / Time.deltaTime;
        prevMousePosition = Input.mousePosition;
    }

    public void StartGame()
    {
        state = GAMEPLAY;
        playerState.StartPlayer();

        GetComponent<BackgroundManager>().enabled = true;
        GetComponent<PlatformManager>().enabled = true;

        welcomeUI.GetComponent<WelcomeDisplay>().Close();

        if (TutorialManager.tutorialEnabled)
        {
            tutorialAnimator.SetTrigger("showTutorial");
            StartCoroutine("GraduallyStopTime");
        }
    }

    public void EndGame()
    {
        state = GAMEOVER;
        playerState.KillPlayer();

        // disable any unnecessary scripts
        GetComponent<PlatformManager>().enabled = false;

        // display game over screen
        gameOverUI.SetActive(true);

        // update high score if changed
        int score = PlatformManager.totalNumObstaclesPassed;
        if (score > GetHighScore())
            SetHighScore(score);

        // turn off tutorial ui if it is currently on
        tutorialAnimator.SetBool("firstSwing", true);
        ResumeNormalTime();

        StartCoroutine("WaitToAllowReset");
    }

    public void ResetGame()
    {
        if (enableResetButton)
            StartCoroutine("TransitionToNewScene");
    }

    IEnumerator TransitionToNewScene()
    {
        transitionAnimator.SetTrigger("exitScene");
        yield return new WaitForSecondsRealtime(0.25f);

        SceneManager.LoadScene("Main");
    }

    IEnumerator WaitToAllowReset()
    {
        yield return new WaitForSecondsRealtime(gameOverAnimationDuration);
        enableResetButton = true;
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    public static void SetHighScore(int highScore)
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    // returns a float from -1.0f to +1.0f
    // -1.0f meaning the cursor is at the leftmost side of the screen
    // +1.0f meaning the cursor is at the rightmost side
    public static Vector2 RelativeMousePosition()
    {
        float x = (Input.mousePosition.x) / (Screen.width) * 2 - 1;
        float y = (Input.mousePosition.y) / (Screen.height) * 2 - 1;
        return new Vector2(x, y);
    }

    IEnumerator GraduallyStopTime()
    {
        float originalTime = Time.timeScale;
        for (int i = 1; i <= 10; i++)
        {
            Time.timeScale = Mathf.Lerp(originalTime, 0f, (float) i / 10);
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }

    public void ResumeNormalTime()
    {
        StopCoroutine("GraduallyStopTime");
        Time.timeScale = 1f;
    }
}
