using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    Vector3 initialPosition;
    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(initialPosition, new Vector3(transform.position.x, 0, 1), 0.95f);
    }
}
