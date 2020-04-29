using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

    public GameObject borderPlatform;
    public GameObject obstaclePlatform;
    public float distanceBetweenBorders;
    public Transform player;

    //GameObject bp1Upper, bp1Lower, bp2Upper, bp2Lower;
    Queue<GameObject> obstacles = new Queue<GameObject>();
    Queue<GameObject> borders = new Queue<GameObject>();

    public int ObstaclesPerBorder;
    public static float spaceBetweenObstacles;
    public float obstacleWidth;
    public int pixelsPerUnit;

    int numOfBorders;
    float borderWidth, borderHeight;
    float upperBound, lowerBound;
    float minHeight, maxHeight;

    public static int numObstaclesPassed;

	// Use this for initialization
	void Start () {
        numOfBorders = 0;
        numObstaclesPassed = 0;
        borderWidth = borderPlatform.GetComponent<Renderer>().bounds.size.x;
        borderHeight = borderPlatform.GetComponent<Renderer>().bounds.size.y;
        spaceBetweenObstacles = borderWidth / ObstaclesPerBorder;
        upperBound = (distanceBetweenBorders / 2 - borderHeight / 2);
        lowerBound = -upperBound;

        addBorderToQueue(-1);
        addBorderToQueue(0);
        addBorderToQueue(1);

        Random.State oldState = Random.state;
        // Seed = 5 seems to be have nice y-positions for easy beginner-friendly obstacles
        Random.InitState(5);
        addObstaclesToQueue(0);

        Random.state = oldState;
        addObstaclesToQueue(1);

    }

    void Update () {
        // FOR KEEPING TRACK OF SCORE
        //Debug.Log((int) ((transform.position.x + obstacleWidth/2) / spaceBetweenObstacles + 0.5f));
        updateNumObstaclesPassed();

        if ((int) (player.position.x /borderWidth) > numOfBorders)
        {
            numOfBorders = (int) (player.position.x / borderWidth);

            destroyBehindBorders();
            addBorderToQueue(numOfBorders + 1);

            if (numOfBorders > 1)
            {
                destroyBehindObstacles();
            }
            addObstaclesToQueue(numOfBorders + 1);
        }
    }

    // even numbered obstacles = potrudes from bottom
    // odd numbered obstacles = potrudes from top
    float randomYPosition()
    {
        return Random.Range(lowerBound, upperBound) * 0.55f;
    }

    GameObject createObstacle(float xPos, float yPos, bool bottom)
    {
        //Debug.Log(xPos + ", " + yPos + ", " + bottom);
        // since unity scales objects relative to their center, the position of the obstacle
        // needs to be offsetted to be in the position I want
        if (bottom)
            yPos = Mathf.Lerp(lowerBound, yPos, 0.5f);
        else
            yPos = Mathf.Lerp(upperBound, yPos, 0.5f);

        float yScale = 2 * (upperBound - Mathf.Abs(yPos)) + 2.0f / pixelsPerUnit;

        GameObject obstacle = (GameObject)Instantiate(obstaclePlatform, new Vector3(xPos, yPos, 0), Quaternion.identity);

        if (!bottom)
            obstacle.GetComponent<SpriteRenderer>().flipY = true;

        // resizing obstacle, setting it in transform won't take advantage of slice mode
        // so we must resize the sprite renderer and boxcollider instead
        obstacle.GetComponent<SpriteRenderer>().size = new Vector2(obstacleWidth, yScale);
        obstacle.GetComponent<BoxCollider2D>().size = new Vector2(obstacleWidth, yScale);
        return obstacle;
    }

    // Overload method for automated randomized obstacles
    GameObject createObstacle(float xPos)
    {
        // even numbered obstacles = potrudes from bottom
        // odd numbered obstacles = potrudes from top
        return createObstacle(xPos, randomYPosition(), obstacles.Count % 2 == 0);
    }

    void updateNumObstaclesPassed()
    {
        int currentNumOsbataclesPassed = (int) ((player.position.x + spaceBetweenObstacles) / spaceBetweenObstacles);

        if (currentNumOsbataclesPassed > numObstaclesPassed)
        {
            numObstaclesPassed = currentNumOsbataclesPassed;
            GameState.canBoost = true;  // player can boost again after passing an obstacle
        }
    }

    void addBorderToQueue(int offset)
    {
        float xCoord = offset * borderWidth;
        float yCoord = distanceBetweenBorders / 2;
        borders.Enqueue((GameObject)Instantiate(borderPlatform, new Vector3(xCoord, yCoord, 0), Quaternion.identity));
        borders.Enqueue((GameObject)Instantiate(borderPlatform, new Vector3(xCoord, -yCoord, 0), Quaternion.identity));
    }

    void addObstaclesToQueue(int offset)
    {
        for (int i = 0; i < ObstaclesPerBorder; i++)
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

    void destroyBehindObstacles()
    {
        for (int i = 0; i < ObstaclesPerBorder; i++)
        {
            Destroy(obstacles.Dequeue());
        }
    }
}
