using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 offset;

    [Range(0f, 1f)]
    public float followSpeed;

    Vector3 destination;

    void Awake()
    {
        transform.position = GetDestination();
    }


    void FixedUpdate () {
        destination = GetDestination();

        // only allow camera to move right
        if (destination.x > transform.position.x)
            transform.position = Vector3.Lerp(transform.position, destination, followSpeed);
	}

    Vector3 GetDestination()
    {
        Vector3 result = player.position + offset;
        result.y = 0;

        return result;
    }
}
