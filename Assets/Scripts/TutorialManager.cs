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

    void Start()
    {
        //tutorialOff = Convert.ToBoolean(PlayerPrefs.GetInt("DisableTutorial"));
        disableTutorialToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("DisableTutorial"));
    }

    void Update()
    {
        if (PlayerState.IsSwinging())
        {
            tutorialAnimator.SetBool("firstSwing", true);
        }

        jumpTutorialAnimator.SetBool("isSwinging", PlayerState.IsSwinging() && GameState.state == GameState.GAMEPLAY);
        if (PlayerState.BoostInput())
            jumpTutorialAnimator.SetBool("firstJump", true);

        // once the player has completed their first swing and jump, they completed the tutorial
        // so this script's duty is finished.
        //if (tutorialAnimator.GetBool("firstSwing") && jumpTutorialAnimator.GetBool("firstJump"))
            //this.enabled = false;
    }

    public void DisableTutorial(bool isOn)
    {
        tutorialOff = isOn;
        jumpTutorialAnimator.SetBool("tutorialOff", tutorialOff);
        if (tutorialOff)
        {
            toggleText.text = "TUTORIAL: OFF";
        } else
        {
            toggleText.text = "TUTORIAL: ON";
        }

        PlayerPrefs.SetInt("DisableTutorial", Convert.ToInt32(tutorialOff));
    }
}
