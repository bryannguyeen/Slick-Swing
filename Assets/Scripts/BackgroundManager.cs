using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    public GameObject backgroundSprite;
    public float parallax;

    Transform cameraT;
    readonly GameObject[] backgrounds = new GameObject[2];

    float backgroundWidth;
    float backgroundRefreshDistance;
    int counter;

    void Start()
    {
        counter = 0;
        cameraT = Camera.main.transform;
        backgroundWidth = backgroundSprite.GetComponent<Renderer>().bounds.size.x;

        backgroundRefreshDistance = backgroundWidth / (1 - parallax);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i] = (GameObject) Instantiate(backgroundSprite);
        }

        FollowCamera();

        this.enabled = false;
    }

    void Update()
    {
        if ((int) (cameraT.position.x / backgroundRefreshDistance) > counter)
        {
            counter = (int)(cameraT.position.x / backgroundRefreshDistance);
        }

        FollowCamera();
    }

    void FollowCamera()
    {
        float xPos;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            xPos = Mathf.Lerp(0, cameraT.position.x, parallax) + (counter + i) * backgroundWidth;
            backgrounds[i].transform.position = new Vector3(xPos, 0, 1);
        }
    }
}
