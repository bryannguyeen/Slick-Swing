using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    public GameObject backgroundSprite;
    public Transform camera;
    public float interpolation;

    List<GameObject> backgrounds = new List<GameObject>();

    float backgroundWidth;
    float backgroundRefreshDistance;
    int counter;

    void Start()
    {
        counter = 0;
        backgroundWidth = backgroundSprite.GetComponent<Renderer>().bounds.size.x;

        backgroundRefreshDistance = backgroundWidth / (1 - interpolation);

        addBackgroundToQueue();
        addBackgroundToQueue();
        FollowCamera();

    }

    void FixedUpdate()
    {
        //Debug.Log(camera.position.x);
        if ((int) (camera.position.x / backgroundRefreshDistance) > counter)
        {
            counter = (int)(camera.position.x / backgroundRefreshDistance);

            destroyEarliestBackground();
            addBackgroundToQueue();
        }

        FollowCamera();
    }

    void FollowCamera()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].transform.position = Vector3.Lerp(new Vector3(0, 0, 1), new Vector3(camera.position.x, 0, 1), interpolation) + new Vector3((counter + i) * backgroundWidth, 0, 0);
        }
    }

    void addBackgroundToQueue()
    {
        backgrounds.Add((GameObject)Instantiate(backgroundSprite, new Vector3(0, 0, 1), Quaternion.identity));
    }

    void destroyEarliestBackground()
    {
        Destroy(backgrounds[0]);
        backgrounds.RemoveAt(0);
    }

    float MathMod(float a, float b)
    {
        return (Mathf.Abs(a * b) + a) % b;
    }
}
