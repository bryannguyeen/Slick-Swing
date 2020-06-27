using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformManager : MonoBehaviour {

    public GameObject borderPlatform;
    public GameObject obstaclePlatform;
    public float distanceBetweenBorders;
    public Transform player;

    Queue<GameObject> obstacles = new Queue<GameObject>();
    Queue<GameObject> borders = new Queue<GameObject>();

    public int ObstaclesPerBorder;
    public static float spaceBetweenObstacles;
    public float obstacleWidth;
    public float obstacleMinHeight;
    public float obstacleMaxHeight;
    public int beginningRandomSeed;

    float pixelsPerUnit;
    float borderWidth, borderHeight;
    float upperBound, lowerBound;

    // To determine whether to spawn the pillar either top or bottom
    bool bottom;

    int numBordersPassed;
    public static int numObstaclesPassed;

	void Start () {
        numBordersPassed = 0;
        numObstaclesPassed = 0;

        pixelsPerUnit = obstaclePlatform.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        borderWidth = borderPlatform.GetComponent<Renderer>().bounds.size.x;
        borderHeight = borderPlatform.GetComponent<Renderer>().bounds.size.y;
        spaceBetweenObstacles = borderWidth / ObstaclesPerBorder;
        upperBound = (distanceBetweenBorders / 2 - borderHeight / 2);
        lowerBound = -upperBound;

        bottom = true;

        AddBorderToQueue(-1);
        AddBorderToQueue(0);
        AddBorderToQueue(1);

        Random.State oldState = Random.state;

        Random.InitState(beginningRandomSeed);
        AddObstaclesToQueue(0);

        Random.state = oldState;
        AddObstaclesToQueue(1);

    }

    void Update () {
        // FOR KEEPING TRACK OF SCORE
        UpdateNumObstaclesPassed();

        if ((int) (player.position.x /borderWidth) > numBordersPassed)
        {
            numBordersPassed = (int) (player.position.x / borderWidth);

            DestroyEarliestBorders();
            AddBorderToQueue(numBordersPassed + 1);

            DestroyEarliestObstacles();
            AddObstaclesToQueue(numBordersPassed + 1);
        }
    }

    float RandomHeight()
    {
        return Random.Range(obstacleMinHeight, obstacleMaxHeight);
    }

    GameObject CreateObstacle(float xPos, float height, bool bottom)
    {
        float yPos;

        // since unity scales objects relative to their center, the position of the obstacle
        // needs to be offsetted to be in the position I want
        if (bottom)
            yPos = lowerBound + height / 2.0f;
        else
            yPos = upperBound - height / 2.0f;

        float yScale = 2 * (upperBound - Mathf.Abs(yPos)) + 4.0f / pixelsPerUnit;

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
    GameObject CreateObstacle(float xPos)
    {
        return CreateObstacle(xPos, RandomHeight(), bottom);
    }

    void UpdateNumObstaclesPassed()
    {
        int currentNumOsbataclesPassed = (int) (player.position.x / spaceBetweenObstacles + 1);

        if (currentNumOsbataclesPassed > numObstaclesPassed)
        {
            numObstaclesPassed = currentNumOsbataclesPassed;
            PlayerState.ReEnableBoost();  // player can boost again after passing an obstacle
        }
    }

    void AddBorderToQueue(int offset)
    {
        float xCoord = offset * borderWidth;
        float yCoord = distanceBetweenBorders / 2;
        borders.Enqueue((GameObject)Instantiate(borderPlatform, new Vector3(xCoord, yCoord, 0), Quaternion.identity));
        borders.Enqueue((GameObject)Instantiate(borderPlatform, new Vector3(xCoord, -yCoord, 0), Quaternion.identity));
    }

    void AddObstaclesToQueue(int offset)
    {
        for (int i = 0; i < ObstaclesPerBorder; i++)
        {
            obstacles.Enqueue(CreateObstacle(offset * borderWidth + (i * spaceBetweenObstacles)));
            bottom = !bottom;   // alternate obstacles from potruding from top and bottom
        }
    }

    void DestroyEarliestBorders()
    {
        for (int i = 0; i < 2; i++) {
            Destroy(borders.Dequeue());
        }
    }

    void DestroyEarliestObstacles()
    {
        for (int i = 0; i < ObstaclesPerBorder; i++)
        {
            Destroy(obstacles.Dequeue());
        }
    }
}
