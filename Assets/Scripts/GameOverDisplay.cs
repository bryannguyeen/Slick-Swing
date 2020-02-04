using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplay : MonoBehaviour
{
    public Text message;
    public Text highScore;
    void Start()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void declareNewRecord()
    {
        message.text = "A NEW RECORD";
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
