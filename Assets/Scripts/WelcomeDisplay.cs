using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeDisplay : MonoBehaviour
{
    public Text HighScoreText;

    private void Start()
    {
        HighScoreText.text = "HIGH SCORE: " + GameState.GetHighScore();
    }

    public void Close()
    {
        StartCoroutine("FadeAndExit");
    }

    IEnumerator FadeAndExit()
    {
        GetComponent<Animator>().SetTrigger("disappear");
        yield return new WaitForSeconds(0.25f);

        gameObject.SetActive(false);
    }
}
