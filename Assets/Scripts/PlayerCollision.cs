using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {

    public PlayerMovement movement;
    public LineRenderer lr;
    public BoxCollider2D boxCollider;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public TrailRenderer trail;
    public AfterimageEffect afterimage;

    public GameObject explosion;


    public GameState gs;

    void OnCollisionEnter2D(Collision2D collisionInfo) {
        KillPlayer();
    }

    void Update ()
    {
        if (Camera.main.transform.position.x - transform.position.x > 20 & !string.Equals(GameState.state, "game over"))
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
        Instantiate(explosion, transform.position, Quaternion.identity);
        afterimage.Stop();

        // update game state
        gs.EndGame();
    }

}
