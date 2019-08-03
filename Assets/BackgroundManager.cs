using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    public GameObject backgroundSprite;
    public float interpolation;

    List<GameObject> backgrounds = new List<GameObject>();

    float backgroundWidth;
    float backgroundRefreshDistance;
    int counter = 0;

    void Start()
    {
        backgroundWidth = backgroundSprite.GetComponent<Renderer>().bounds.size.x;

        backgroundRefreshDistance = backgroundWidth / (1 - interpolation);

        addBackgroundToQueue();
        addBackgroundToQueue();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(transform.position.x);
        if ((int) (transform.position.x / backgroundRefreshDistance) > counter)
        {
            counter = (int)(transform.position.x / backgroundRefreshDistance);

            destroyEarliestBackground();
            addBackgroundToQueue();
        }

        for(int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].transform.position = Vector3.Lerp(new Vector3(0, 0, 1), new Vector3(transform.position.x, 0, 1), interpolation) + new Vector3((counter + i) * backgroundWidth, 0, 0);
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
