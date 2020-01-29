using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public Transform camera;

    Vector3 initialPosition;
    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(initialPosition, new Vector3(camera.position.x, 0, 1), 0.95f);
    }
}
