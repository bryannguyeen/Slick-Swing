
using UnityEngine;

[System.Serializable]
public class Background
{
    public GameObject spriteObject;

    [Range(0f, 1f)]
    public float parallax;

    [HideInInspector]
    public float width;

    [HideInInspector]
    public float loopDistance;

    [HideInInspector]
    public GameObject[] instances = new GameObject[2];

    public int GetNumBackgroundLoops(Vector3 cameraPosition)
    {
        return (int)Mathf.Floor(cameraPosition.x / loopDistance);
    }

    public void FollowCamera(Vector3 cameraPosition)
    {
        // set the first background position
        int offset = GetNumBackgroundLoops(cameraPosition);
        float xPos = Mathf.Lerp(0, cameraPosition.x, parallax) + offset * width;
        instances[0].transform.position = new Vector3(xPos, 0, 1);

        // set the subsequent backgrounds ahead and side-to-side of the first background
        for (int i = 1; i < instances.Length; i++)
        {
            instances[i].transform.position = new Vector3(xPos + (i * width), 0, 1);
        }
    }
}
