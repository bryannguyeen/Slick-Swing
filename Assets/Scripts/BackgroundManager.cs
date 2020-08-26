using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    public GameObject backgroundSprite;

    [Range(0f, 1f)]
    public float parallax;

    Transform cameraT;

    public Background[] backgroundLayers;
    readonly GameObject[] backgrounds = new GameObject[2];

    float backgroundWidth;
    float backgroundLoopDistance;
    int offset;

    void Awake()
    {
        cameraT = Camera.main.transform;

        foreach (Background b in backgroundLayers)
        {
            b.width = b.spriteObject.GetComponent<Renderer>().bounds.size.x;
            b.loopDistance = b.width / (1 - b.parallax);
            b.offset = b.GetNumBackgroundLoops(cameraT.position);

            for (int i = 0; i < b.instances.Length; i++)
            {
                b.instances[i] = (GameObject)Instantiate(b.spriteObject);
            }

            b.FollowCamera(cameraT.position);
        }

        /*
        backgroundWidth = backgroundSprite.GetComponent<Renderer>().bounds.size.x;
        backgroundLoopDistance = backgroundWidth / (1 - parallax);

        offset = GetNumBackgroundLoops(cameraT.position, backgroundLoopDistance);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i] = (GameObject) Instantiate(backgroundSprite);
        }

        FollowCamera();
        */

        this.enabled = false;
    }

    void Update()
    {
        foreach (Background b in backgroundLayers)
        {
            b.offset = b.GetNumBackgroundLoops(cameraT.position);

            b.FollowCamera(cameraT.position);
        }

        /*
        offset = GetNumBackgroundLoops(cameraT.position, backgroundLoopDistance);

        FollowCamera();
        */
    }

    void FollowCamera()
    {
        // set the first background position
        float xPos = Mathf.Lerp(0, cameraT.position.x, parallax) + offset * backgroundWidth;
        backgrounds[0].transform.position = new Vector3(xPos, 0, 1);

        // set the subsequent backgrounds ahead and side-to-side of the first background
        for (int i = 1; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.position = new Vector3(xPos + (i * backgroundWidth), 0, 1);
        }
    }

    int GetNumBackgroundLoops(Vector3 cameraPosition, float loopLength)
    {
        return (int)Mathf.Floor(cameraPosition.x / loopLength);
    }
}
