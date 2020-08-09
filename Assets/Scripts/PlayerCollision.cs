using UnityEngine;

public class PlayerCollision : MonoBehaviour {
    public GameState gs;
    void OnCollisionEnter2D(Collision2D collisionInfo) {
        if (collisionInfo.collider.CompareTag("Platform"))
            gs.EndGame();
    }

    void Update ()
    {
        if (Camera.main.transform.position.x - transform.position.x > 20 & GameState.state == GameState.GAMEPLAY)
            gs.EndGame();
    }
}
