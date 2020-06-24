using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplay : MonoBehaviour
{
    public Text message;
    public Text messageShadow;
    public Text highScore;

    public Color newRecordColor;
    void Start()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    public void DeclareNewRecord()
    {
        message.text = "A NEW RECORD";
        messageShadow.text = message.text;
        messageShadow.color = newRecordColor;
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
