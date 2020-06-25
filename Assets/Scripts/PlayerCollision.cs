using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
    public GameState gs;
    void OnCollisionEnter2D(Collision2D collisionInfo) {
        gs.EndGame();
    }

    void Update ()
    {
        if (Camera.main.transform.position.x - transform.position.x > 20 & GameState.state == GameState.GAMEPLAY)
            gs.EndGame();
    }
}
