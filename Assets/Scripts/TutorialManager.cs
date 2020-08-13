using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static bool tutorialEnabled;
    public Text toggleText;
    public Toggle tutorialToggle;

    public GameObject swingTutorialUI;
    public GameObject jumpTutorialUI;

    void Start()
    {
        // Tutorial is on by default and then checks PlayerPrefs too see whether to disable it
        tutorialEnabled = tutorialToggle.isOn = GetDisableTutorialPlayerPref();
    }

    void Update()
    {
        if (GameState.state != GameState.START)
        {
            // Disable script after completing tutorial or tutorial is disabled in gameplay
            // Completion of the jump tutorial already implies completion of swing tutorial and that
            // GameState.state is in GAMEPLAY
            if (jumpTutorialUI.GetComponent<Animator>().GetBool("firstJump") || !tutorialEnabled)
                StartCoroutine("ExitTutorial");

            if (PlayerState.mouseClick)
            {
                swingTutorialUI.GetComponent<Animator>().SetBool("firstSwing", true);
                GetComponent<GameState>().ResumeNormalTime();
            }

            jumpTutorialUI.GetComponent<Animator>().SetBool("isSwinging", PlayerState.IsSwinging() && GameState.state == GameState.GAMEPLAY);
            if (PlayerState.BoostInput())
                jumpTutorialUI.GetComponent<Animator>().SetBool("firstJump", true);
        }
    }

    public void DisableTutorial(bool isOn)
    {
        tutorialEnabled = isOn;
        jumpTutorialUI.GetComponent<Animator>().SetBool("tutorialEnabled", tutorialEnabled);
        if (tutorialEnabled)
        {
            toggleText.text = "TUTORIAL: ON";
        }
        else
        {
            toggleText.text = "TUTORIAL: OFF";
        }

        SetDisableTutorialPlayerPref(tutorialEnabled);
    }

    bool GetDisableTutorialPlayerPref()
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt("tutorialEnabled", 1));
    }

    void SetDisableTutorialPlayerPref(bool disable)
    {
        PlayerPrefs.SetInt("tutorialEnabled", Convert.ToInt32(disable));
    }

    IEnumerator ExitTutorial()
    {
        yield return new WaitForSecondsRealtime(0.75f);
        swingTutorialUI.SetActive(false);
        jumpTutorialUI.SetActive(false);
        this.enabled = false;
    }
}
