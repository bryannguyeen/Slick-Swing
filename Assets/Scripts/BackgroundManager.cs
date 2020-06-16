using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    public GameObject backgroundSprite;
    public float parallax;

    Transform cameraT;
    List<GameObject> backgrounds = new List<GameObject>();

    float backgroundWidth;
    float backgroundRefreshDistance;
    int counter;

    void Start()
    {
        counter = 0;
        cameraT = Camera.main.transform;
        backgroundWidth = backgroundSprite.GetComponent<Renderer>().bounds.size.x;

        backgroundRefreshDistance = backgroundWidth / (1 - parallax);

        AddBackgroundToQueue();
        AddBackgroundToQueue();
        FollowCamera();

    }

    void Update()
    {
        if ((int) (cameraT.position.x / backgroundRefreshDistance) > counter)
        {
            counter = (int)(cameraT.position.x / backgroundRefreshDistance);

            DestroyEarliestBackground();
            AddBackgroundToQueue();
        }

        FollowCamera();
    }

    void FollowCamera()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].transform.position = Vector3.Lerp(new Vector3(0, 0, 1), new Vector3(cameraT.position.x, 0, 1), parallax) + new Vector3((counter + i) * backgroundWidth, 0, 0);
        }
    }

    void AddBackgroundToQueue()
    {
        backgrounds.Add((GameObject)Instantiate(backgroundSprite, new Vector3(0, 0, 1), Quaternion.identity));
    }

    void DestroyEarliestBackground()
    {
        Destroy(backgrounds[0]);
        backgrounds.RemoveAt(0);
    }
}
