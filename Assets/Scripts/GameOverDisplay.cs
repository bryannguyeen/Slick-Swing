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

    private void Awake()
    {
        oldHighScore = GameState.GetHighScore();
        highScore.text = oldHighScore.ToString();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // check if the score is a new record
        if (PlatformManager.numObstaclesPassed > oldHighScore)
            DeclareNewRecord();
    }

    void DeclareNewRecord()
    {
        message.text = "A NEW RECORD";
        messageShadow.text = message.text;
        messageShadow.color = newRecordColor;
        newHighScore = PlatformManager.numObstaclesPassed;

        StartCoroutine("HighScoreChangeAnimation");
    }

    IEnumerator HighScoreChangeAnimation()
    {
        yield return new WaitForSecondsRealtime(0.65f);
        float incrementPeriod = 0.3f / (newHighScore - oldHighScore);

        for (int i = oldHighScore + 1; i <= newHighScore; i++)
        {
            yield return new WaitForSecondsRealtime(incrementPeriod);
            highScore.text = i.ToString();
        }
        GetComponent<Animator>().SetTrigger("Glow high score");
    }
}
