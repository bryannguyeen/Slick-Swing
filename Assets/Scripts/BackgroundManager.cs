using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    public GameObject backgroundSprite;

    [Range(0f, 1f)]
    public float parallax;

    Transform cameraT;

    readonly GameObject[] backgrounds = new GameObject[2];

    float backgroundWidth;
    float backgroundLoopDistance;
    int offset;

    void Start()
    {
        cameraT = Camera.main.transform;
        backgroundWidth = backgroundSprite.GetComponent<Renderer>().bounds.size.x;
        backgroundLoopDistance = backgroundWidth / (1 - parallax);

        offset = GetNumBackgroundLoops(cameraT.position, backgroundLoopDistance);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i] = (GameObject) Instantiate(backgroundSprite);
        }

        FollowCamera();

        this.enabled = false;
    }

    void Update()
    {
        offset = GetNumBackgroundLoops(cameraT.position, backgroundLoopDistance);

        FollowCamera();
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
