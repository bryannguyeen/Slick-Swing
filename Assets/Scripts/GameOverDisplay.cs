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
        highScore.text = GameState.GetHighScore().ToString();
    }

    public void DeclareNewRecord()
    {
        message.text = "A NEW RECORD";
        messageShadow.text = message.text;
        messageShadow.color = newRecordColor;
        highScore.text = GameState.GetHighScore().ToString();
    }
}
