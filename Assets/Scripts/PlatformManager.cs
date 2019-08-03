using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

    public Sprite upperObstacle;
    public GameObject borderPlatform;
    public GameObject obstaclePlatform;
    public float distanceBetweenBorders;
    public Transform player;

    //GameObject bp1Upper, bp1Lower, bp2Upper, bp2Lower;
    Queue<GameObject> obstacles = new Queue<GameObject>();
    Queue<GameObject> borders = new Queue<GameObject>();

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

        addBorderToQueue(-1);
        addBorderToQueue(0);
        addBorderToQueue(1);

        addObstaclesToQueue(0);
        addObstaclesToQueue(1);

    }

    void Update () {
        // FOR KEEPING TRACK OF SCORE
        //Debug.Log((int) ((transform.position.x + obstacleWidth/2) / spaceBetweenObstacles + 0.5f));
        updateNumObstaclesPassed();

        if ((int) (player.position.x /borderWidth) > counter)
        {
            counter = (int) (player.position.x / borderWidth);

            destroyBehindBorders();
            addBorderToQueue(counter + 1);

            if (counter > 1)
            {
                destroyBehindObsticles();
            }
            addObstaclesToQueue(counter + 1);
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
        int currentNum = (int) ((player.position.x + spaceBetweenObstacles) / spaceBetweenObstacles);

        if (currentNum > numObstaclesPassed)
            numObstaclesPassed = currentNum;
    }

    void addBorderToQueue(int offset)
    {
        float xCoord = offset * borderWidth;
        float yCoord = distanceBetweenBorders / 2;
        GameObject upperPlatform = (GameObject)Instantiate(borderPlatform, new Vector3(xCoord, yCoord, 0), Quaternion.identity);
        GameObject lowerPlatform = (GameObject)Instantiate(borderPlatform, new Vector3(xCoord, -yCoord, 0), Quaternion.identity);

        borders.Enqueue(upperPlatform);
        borders.Enqueue(lowerPlatform);
    }

    void addObstaclesToQueue(int offset)
    {
        for (int i = 0; i < obstaclesPerScreen; i++)
        {
            obstacles.Enqueue(createObstacle(offset * borderWidth + (i * spaceBetweenObstacles)));
        }
    }

    void destroyBehindBorders()
    {
        for (int i = 0; i < 2; i++) {
            Destroy(borders.Dequeue());
        }
    }

    void destroyBehindObsticles()
    {
        for (int i = 0; i < obstaclesPerScreen; i++)
        {
            Destroy(obstacles.Dequeue());
        }
    }
}
