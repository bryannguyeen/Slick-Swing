﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlipPlayer : MonoBehaviour
{
    public Animator animator;

    public void Play(Vector2 position)
    {
        transform.position = (Vector3) position;
        animator.Play("blip", -1, 0);
    }
}
