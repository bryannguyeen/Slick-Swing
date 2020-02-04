using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 offset;

    Vector3 destination;

    void Start()
    {
        transform.position = getDestination();
        Debug.Log(getDestination());
    }


    void FixedUpdate () {
        destination = getDestination();

        // only allow camera to move right
        if (destination.x > transform.position.x)
            transform.position = Vector3.Lerp(transform.position, destination, 0.1f);
	}

    Vector3 getDestination()
    {
        Vector3 result = player.position + offset;
        result.y = 0;

        return result;
    }
}
