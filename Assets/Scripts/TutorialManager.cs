using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static bool tutorialOff;
    public Text toggleText;
    public Toggle disableTutorialToggle;

    public Animator tutorialAnimator;
    public Animator jumpTutorialAnimator;

    [Range(0.2f, 1f)]
    public float tutorialTimeScale;

    void Start()
    {
        // Tutorial is on by default and then checks PlayerPrefs too see whether to disable it
        Time.timeScale = tutorialTimeScale;

        tutorialOff = disableTutorialToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("DisableTutorial"));
    }

    void Update()
    {
        // no need to show tutorial prompts once player completes tutorial
        if ((tutorialAnimator.GetBool("firstSwing") && jumpTutorialAnimator.GetBool("firstJump")))
            return;

        if (PlayerState.IsCasting())
        {
            tutorialAnimator.SetBool("firstSwing", true);
            Time.timeScale = 1f;
        }

        jumpTutorialAnimator.SetBool("isSwinging", PlayerState.IsSwinging() && GameState.state == GameState.GAMEPLAY);
        if (PlayerState.BoostInput())
            jumpTutorialAnimator.SetBool("firstJump", true);
    }

    public void DisableTutorial(bool isOn)
    {
        tutorialOff = isOn;
        jumpTutorialAnimator.SetBool("tutorialOff", tutorialOff);
        if (tutorialOff)
        {
            toggleText.text = "TUTORIAL: OFF";
            Time.timeScale = 1f;
        }
        else
        {
            toggleText.text = "TUTORIAL: ON";
            Time.timeScale = tutorialTimeScale;
        }

        PlayerPrefs.SetInt("DisableTutorial", Convert.ToInt32(tutorialOff));
    }
}
