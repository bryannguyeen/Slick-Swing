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

    void Start()
    {
        tutorialOff = Convert.ToBoolean(PlayerPrefs.GetInt("DisableTutorial"));
        disableTutorialToggle.isOn = tutorialOff;
    }

    void Update()
    {
        if (GameState.state == GameState.GAMEPLAY && Input.GetMouseButtonDown(0))
        {
            tutorialAnimator.SetBool("firstSwing", true);
        }
    }

    public void DisableTutorial(bool isOn)
    {
        tutorialOff = isOn;
        if (tutorialOff)
        {
            toggleText.text = "TUTORIAL: OFF";
        } else
        {
            toggleText.text = "TUTORIAL: ON";
        }

        PlayerPrefs.SetInt("DisableTutorial", Convert.ToInt32(isOn));
    }
}
