using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    Transform cameraT;
    public Background[] backgroundLayers;

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

        this.enabled = false;
    }

    void Update()
    {
        foreach (Background b in backgroundLayers)
        {
            b.offset = b.GetNumBackgroundLoops(cameraT.position);

            b.FollowCamera(cameraT.position);
        }
    }
}
