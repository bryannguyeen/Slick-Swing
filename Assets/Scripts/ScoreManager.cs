using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreBox;

    void Update()
    {
        scoreBox.text = PlatformManager.totalNumObstaclesPassed.ToString();
    }
}
