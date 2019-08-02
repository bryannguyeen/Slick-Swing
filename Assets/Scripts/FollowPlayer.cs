using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        transform.position = getDestination();
    }


    // Update is called once per frame
    void FixedUpdate () {
        transform.position = Vector3.Lerp(transform.position, getDestination(), 0.1f);
	}

    Vector3 getDestination()
    {
        Vector3 result = player.position + offset;
        result.y = 0;

        return result;
    }
}
