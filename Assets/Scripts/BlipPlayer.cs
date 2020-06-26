using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlipPlayer : MonoBehaviour
{
    public void Play(Vector2 position)
    {
        transform.position = (Vector3) position;
        GetComponent<Animator>().SetTrigger("playBlip");
    }
}
