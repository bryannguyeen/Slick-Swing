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

    GameState gs;

    [Range(0.2f, 1f)]
    public float tutorialTimeScale;

    void Start()
    {
        gs = GetComponent<GameState>();

        // Tutorial is on by default and then checks PlayerPrefs too see whether to disable it
        tutorialOff = disableTutorialToggle.isOn = GetDisableTutorialPlayerPref();
    }

    void Update()
    {
        // Disable script after completing tutorial
        // Completion of the jump tutorial already implies completion of swing tutorial and that
        // GameState.state is in GAMEPLAY
        if (jumpTutorialAnimator.GetBool("firstJump"))
            this.enabled = false;

        if (PlayerState.mouseClick)
        {
            tutorialAnimator.SetBool("firstSwing", true);
            gs.ResumeNormalTime();
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
        }
        else
        {
            toggleText.text = "TUTORIAL: ON";
        }

        SetDisableTutorialPlayerPref(tutorialOff);
    }

    bool GetDisableTutorialPlayerPref()
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt("DisableTutorial"));
    }

    void SetDisableTutorialPlayerPref(bool disable)
    {
        PlayerPrefs.SetInt("DisableTutorial", Convert.ToInt32(disable));
    }
}
