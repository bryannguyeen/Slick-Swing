using System;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public const int BEGINNING = 0;
    public const int FREEFALL = 1;
    public const int CASTING = 2;
    public const int SWINGING = 3;

    private static int state;
    private static bool canBoost;

    public static bool mouseClick;    // is true only if the mouse is clicked on current frame

    public static bool mouseRelease;  // is true only if the mouse is released on current frame
    public static bool mouseHold;     // is true as long as the mouse is pressed, false otherwise

    public static float minFingerBoostDistance = 20f;
    public static float maxFingerBoostDistance = 120f;

    public static Vector3 clickPosition;

    PlayerMovement movement;
    PlayerCollision collision;
    LineRenderer lr;
    BoxCollider2D boxCollider;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    TrailRenderer trail;
    AfterimageEffect afterimage;
    Animator animator;

    public AudioManager audioManager;
    public GameObject explosion;

    public GameState gs;

    public Vector2 startingJump;


    void Start()
    {
        state = BEGINNING;
        canBoost = true;
        mouseClick = false;
        mouseRelease = false;
        mouseHold = false;

        movement = GetComponent<PlayerMovement>();
        collision = GetComponent<PlayerCollision>();
        lr = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        afterimage = GetComponent<AfterimageEffect>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameState.state == GameState.GAMEPLAY)
        {
            CheckForMouseClick();
            CheckForMouseRelease();
        }
    }

    void CheckForMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseClick = true;
            mouseHold = true;
            clickPosition = Input.mousePosition;
        }
    }

    void CheckForMouseRelease()
    {
        if (Input.GetMouseButtonUp(0) && mouseHold)
        {
            mouseRelease = true;
            mouseHold = false;
        }
    }

    public void StartPlayer()
    {
        PlayerState.SetToFreefall();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.AddForce(startingJump);
        animator.SetTrigger("doBackflip");
        audioManager.Play("BigLeap");

        movement.enabled = true;
        collision.enabled = true;
    }

    public void KillPlayer()
    {
        trail.enabled = false;
        sprite.enabled = false;
        movement.enabled = false;
        collision.enabled = false;
        lr.enabled = false;
        boxCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Instantiate(explosion, transform.position, Quaternion.identity);
        afterimage.Stop();
        audioManager.Play("Explosion");
    }

    public static int GetState()
    {
        return state;
    }

    public static bool IsStarting()
    {
        return state == BEGINNING;
    }

    public static bool IsFreefall()
    {
        return state == FREEFALL;
    }

    public static bool IsCasting()
    {
        return state == CASTING;
    }

    public static bool IsSwinging()
    {
        return state == SWINGING;
    }

    public static void SetToFreefall()
    {
        state = FREEFALL;
    }

    public static void SetToCasting()
    {
        state = CASTING;
    }

    public static void SetToSwinging()
    {
        state = SWINGING;
    }

    public static bool BoostInput()
    {
        return canBoost && IsSwinging() && DistanceFromMouseclick() > minFingerBoostDistance;
    }

    public static void DisableBoost()
    {
        canBoost = false;
    }

    public static void ReEnableBoost()
    {
        canBoost = true;
    }

    public static bool CanBoost()
    {
        return canBoost;
    }

    public static Vector2 BoostDirection()
    {
        return (Input.mousePosition - clickPosition).normalized;
    }

    public static float DistanceFromMouseclick()
    {
        return (Input.mousePosition - clickPosition).magnitude;
    }
}
