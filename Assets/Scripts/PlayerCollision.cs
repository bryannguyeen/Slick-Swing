﻿using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

    public PlayerMovement movement;
    public LineRenderer lr;
    public BoxCollider2D boxCollider;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public TrailRenderer trail;
    public ParticleSystem particles;

    void OnCollisionEnter2D(Collision2D collisionInfo) {
        Debug.Log(collisionInfo.collider.tag);
        killPlayer();
    }

    void killPlayer()
    {
        trail.enabled = false;
        sprite.enabled = false;
        movement.enabled = false;
        lr.enabled = false;
        boxCollider.enabled = false;
        rb.velocity = new Vector2(0, 0);
        particles.Play();
    }

}
