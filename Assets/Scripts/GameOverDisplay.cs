using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplay : MonoBehaviour
{
    public Text message;
    public Text messageShadow;
    public Text highScore;

    private int oldHighScore, newHighScore;

    public Color newRecordColor;

    // Awake and not Start to ensure that oldHighScore is
    // saved before GameState updates it
    private void Awake()
    {
        oldHighScore = GameState.GetHighScore();
        highScore.text = oldHighScore.ToString();
    }

    public void DeclareNewRecord()
    {
        message.text = "A NEW RECORD";
        messageShadow.text = message.text;
        messageShadow.color = newRecordColor;
        newHighScore = GameState.GetHighScore();
        StartCoroutine("HighScoreChangeAnimation");
    }

    IEnumerator HighScoreChangeAnimation()
    {
        yield return new WaitForSecondsRealtime(0.65f);
        float pauseTime = 0.3f / (newHighScore - oldHighScore);

        for (int i = oldHighScore + 1; i <= newHighScore; i++)
        {
            yield return new WaitForSecondsRealtime(pauseTime);
            highScore.text = i.ToString();
        }
        GetComponent<Animator>().SetTrigger("Glow high score");
    }
}
