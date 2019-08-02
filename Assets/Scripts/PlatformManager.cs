using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

    public Sprite upperObstacle;
    public GameObject borderPlatform;
    public GameObject obstaclePlatform;
    public float distanceBetweenBorders;

    GameObject bp1Upper, bp1Lower, bp2Upper, bp2Lower;
    List<GameObject> obstacles = new List<GameObject>();
    public int obstaclesPerScreen;
    public static float spaceBetweenObstacles;
    public float obstacleWidth;
    public int pixelsPerUnit;

    int counter;
    float borderWidth, borderHeight;
    float upperBound, lowerBound;

    public static int numObstaclesPassed = 0;

	// Use this for initialization
	void Start () {
        counter = 0;
        Random.InitState(5);
        borderWidth = borderPlatform.GetComponent<Renderer>().bounds.size.x;
        borderHeight = borderPlatform.GetComponent<Renderer>().bounds.size.y;
        spaceBetweenObstacles = borderWidth / obstaclesPerScreen;
        upperBound = (distanceBetweenBorders / 2 - borderHeight / 2);
        lowerBound = -upperBound;

        bp1Upper = (GameObject)Instantiate(borderPlatform, new Vector3(0, distanceBetweenBorders/2, 0), Quaternion.identity);
        bp1Lower = (GameObject)Instantiate(borderPlatform, new Vector3(0, -distanceBetweenBorders/2, 0), Quaternion.identity);
        bp2Upper = (GameObject)Instantiate(borderPlatform, new Vector3(borderWidth, distanceBetweenBorders/2, 0), Quaternion.identity);
        bp2Lower = (GameObject)Instantiate(borderPlatform, new Vector3(borderWidth, -distanceBetweenBorders/2, 0), Quaternion.identity);

        for (int i = 0; i < obstaclesPerScreen * 3; i++)
        {
            obstacles.Add(createObstacle(i * spaceBetweenObstacles));
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        // FOR KEEPING TRACK OF SCORE
        //Debug.Log((int) ((transform.position.x + obstacleWidth/2) / spaceBetweenObstacles + 0.5f));
        updateNumObstaclesPassed();

        if ((int) (transform.position.x /borderWidth) > counter)
        {
            counter = (int) (transform.position.x / borderWidth);

            Destroy(bp1Upper);
            Destroy(bp1Lower);
            bp1Upper = bp2Upper;
            bp1Lower = bp2Lower;

            bp2Upper = (GameObject)Instantiate(borderPlatform, new Vector3((counter + 1) * borderWidth, distanceBetweenBorders / 2, 0), Quaternion.identity);
            bp2Lower = (GameObject)Instantiate(borderPlatform, new Vector3((counter + 1) * borderWidth, -distanceBetweenBorders / 2, 0), Quaternion.identity);

            if (counter > 1)
            {
                for (int i = 0; i < obstaclesPerScreen; i++)
                {
                    Destroy(obstacles[0]);
                    obstacles.RemoveAt(0);
                }
            }

            for (int i = 0; i < obstaclesPerScreen; i++)
            {
                obstacles.Add(createObstacle((counter + 2) * borderWidth + (i * spaceBetweenObstacles)));
            }
        }
    }

    Vector3 randomPosition(float xPosition)
    {
        float y = 0.5f *(Random.Range(-distanceBetweenBorders / 6.0f, distanceBetweenBorders / 6.0f) + lowerBound);
        return new Vector3(xPosition, y, 0);
    }

    // even numbered obstacles = potrudes from bottom
    // odd numbered obstacles = potrudes from top
    float randomY()
    {
        if (obstacles.Count % 2 == 0)
            return 0.5f * (Random.Range(-distanceBetweenBorders / 5.0f, distanceBetweenBorders / 5.0f) + lowerBound);
        return 0.5f * (Random.Range(-distanceBetweenBorders / 5.0f, distanceBetweenBorders / 5.0f) + upperBound);
    }

    GameObject createObstacle(float xPosition)
    {
        float yPosition = randomY();
        float yScale = 2 * (upperBound -  Mathf.Abs(yPosition)) + 2.0f / pixelsPerUnit;

        GameObject obstacle = (GameObject)Instantiate(obstaclePlatform, new Vector3(xPosition, yPosition, 0), Quaternion.identity);

        if (obstacles.Count % 2 == 1)
            obstacle.GetComponent<SpriteRenderer>().sprite = upperObstacle;

        // resizing obstacle
        obstacle.GetComponent<SpriteRenderer>().size = new Vector2(obstacleWidth, yScale);
        obstacle.GetComponent<BoxCollider2D>().size = new Vector2(obstacleWidth, yScale);
        return obstacle;
    }

    void updateNumObstaclesPassed()
    {
        int currentNum = (int) ((transform.position.x + obstacleWidth / 2) / spaceBetweenObstacles + 0.5f);

        if (currentNum > numObstaclesPassed)
            numObstaclesPassed = currentNum;
    }
}
