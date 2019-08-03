using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreBox;
    // Update is called once per frame
    void Update()
    {
        scoreBox.text = PlatformManager.numObstaclesPassed.ToString();
    }
}
