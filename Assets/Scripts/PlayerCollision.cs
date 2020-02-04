using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

    public PlayerMovement movement;
    public LineRenderer lr;
    public BoxCollider2D boxCollider;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public TrailRenderer trail;
    public ParticleSystem particles;

    public GameState gs;

    public Transform camera;

    void OnCollisionEnter2D(Collision2D collisionInfo) {
        KillPlayer();
    }

    void FixedUpdate ()
    {
        if (camera.position.x - transform.position.x > 20 & !string.Equals(GameState.state, "game over"))
            KillPlayer();
    }

    void KillPlayer()
    {
        trail.enabled = false;
        sprite.enabled = false;
        movement.enabled = false;
        lr.enabled = false;
        boxCollider.enabled = false;
        rb.velocity = new Vector2(0, 0);
        particles.Play();

        // update game state
        gs.EndGame();
    }

}
