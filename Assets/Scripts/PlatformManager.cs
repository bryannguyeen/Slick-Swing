﻿using UnityEngine;
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

    float pixelsPerUnit;
    Vector2 borderSize;
    float upperBound, lowerBound;

    // To determine whether to spawn the pillar either top or bottom
    bool bottom;

    int numBordersPassed;
    public static int totalNumObstaclesPassed;


    void Awake() {
        numBordersPassed = 0;
        totalNumObstaclesPassed = 0;
        pixelsPerUnit = obstaclePlatform.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        borderSize = borderPlatform.GetComponentInChildren<Renderer>().bounds.size;
        spaceBetweenObstacles = borderSize.x / ObstaclesPerBorder;
        upperBound = (distanceBetweenBorders / 2 - borderSize.y / 2);
        lowerBound = -upperBound;

        bottom = true;

        for (int i = -1; i < 2; i++)
            AddBorderToQueue(i);

        for (int i = 0; i < 2; i++)
        AddObstaclesToQueue(i);

        this.enabled = false;
    }

    void Update() {
        // FOR KEEPING TRACK OF SCORE
        UpdateNumObstaclesPassed();

        if ((int) (player.position.x / borderSize.x) > numBordersPassed)
        {
            numBordersPassed = (int) (player.position.x / borderSize.x);

            DestroyEarliestBorders();
            if (numBordersPassed > 1)
                DestroyEarliestObstacles();

            LoadBorderObstacleSet(numBordersPassed + 1);
        }
    }

    float RandomHeight()
    {
        // difficultyScale will make the obstacle heights shorter for the early game
        float difficultyScale = Mathf.Lerp(0.55f, 1.0f, Mathf.Min(numBordersPassed, 10f) / 10f);
        return Random.Range(obstacleMinHeight, obstacleMaxHeight) * difficultyScale;
    }

    GameObject CreateObstacle(float xPos, float obstacleHeight, bool bottom)
    {
        float yPos;
        Quaternion rotation;

        // place position of obstacle to be either from top or bottom
        if (bottom)
        {
            yPos = lowerBound - 2 / pixelsPerUnit;
            rotation = Quaternion.identity;
        }
        else
        {
            yPos = upperBound + 2 / pixelsPerUnit;
            rotation = Quaternion.Euler(180f, 0f, 0f);
        }

        GameObject obstacle = (GameObject) Instantiate(obstaclePlatform, new Vector3(xPos, yPos, 0), rotation);

        // resizing obstacle, setting it in transform won't take advantage of slice mode
        // so we must resize the sprite renderer instead
        obstacle.GetComponent<SpriteRenderer>().size = new Vector2(obstacleWidth, obstacleHeight);

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

        if (currentNumOsbataclesPassed > totalNumObstaclesPassed)
        {
            totalNumObstaclesPassed = currentNumOsbataclesPassed;
            PlayerState.ReEnableBoost();  // player can boost again after passing an obstacle
        }
    }

    void LoadBorderObstacleSet(int offset)
    {
        AddBorderToQueue(offset);
        AddObstaclesToQueue(offset);
    }

    void AddBorderToQueue(int offset)
    {
        float xCoord = offset * borderSize.x;
        float yCoord = distanceBetweenBorders / 2;
        borders.Enqueue((GameObject)Instantiate(borderPlatform, new Vector3(xCoord, yCoord, 0), Quaternion.identity));
        borders.Enqueue((GameObject)Instantiate(borderPlatform, new Vector3(xCoord, -yCoord, 0), Quaternion.identity));
    }

    void AddObstaclesToQueue(int offset)
    {
        for (int i = 0; i < ObstaclesPerBorder; i++)
        {
            obstacles.Enqueue(CreateObstacle(offset * borderSize.x + (i * spaceBetweenObstacles)));
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
