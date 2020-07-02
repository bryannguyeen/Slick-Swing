﻿using System;
using System.Collections;
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

    public float gameOverAnimationDuration;

    public Animator tutorialAnimator;

    bool enableResetButton;

    void Start()
    {
        state = START;
        prevMousePosition = Input.mousePosition;
        HighScoreText.text = "HIGH SCORE: " + GetHighScore();

        enableResetButton = false;
    }

    void Update()
    {
        cursorVelocity = ((Vector2)Input.mousePosition - prevMousePosition) / Time.deltaTime;
    }

    private void LateUpdate()
    {
        // previous mouse position should be recorded at the end of the frame
        prevMousePosition = Input.mousePosition;
    }

    public void StartGame()
    {
        state = GAMEPLAY;
        playerState.StartPlayer();

        GetComponent<BackgroundManager>().enabled = true;
        GetComponent<PlatformManager>().enabled = true;

        StartCoroutine("HideWelcomeUI");

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

        // update high score
        int score = PlatformManager.numObstaclesPassed;
        if (score > GetHighScore())
        {
            SetHighScore(score);
            gameOverUI.GetComponent<GameOverDisplay>().DeclareNewRecord();
        }

        // display game over screen
        gameOverUI.SetActive(true);

        // turn of tutorial ui if it is currently on
        tutorialAnimator.SetBool("firstSwing", true);
        ResumeNormalTime();

        StartCoroutine("WaitToAllowReset");
    }

    public void ResetGame()
    {
        if (enableResetButton)
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
    public static float RelativeMousePositionX()
    {
        return (Input.mousePosition.x) / (Screen.width) * 2 - 1;
    }

    public static float RelativeMousePositionY()
    {
        return (Input.mousePosition.y) / (Screen.height) * 2 - 1;
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

    IEnumerator HideWelcomeUI()
    {
        welcomeUI.GetComponent<Animator>().SetTrigger("disappear");
        yield return new WaitForSeconds(0.25f);

        welcomeUI.SetActive(false);
    }

    public void ResumeNormalTime()
    {
        StopCoroutine("GraduallyStopTime");
        Time.timeScale = 1f;
    }
}
