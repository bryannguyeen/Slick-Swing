using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public LineRenderer lr;
    public TrailRenderer trail;
    public Animator animator;

    public float bungeeStiffness;
    public float dampingConstant;
    public float boostScale;
    public float ropeCooldown;

    public Vector2 startingPosition;
    public Vector2 startingJump;

    Vector2 connectionPoint;
    float bungeeLength;
    Vector2 netForce;
    Vector2 netBurstForce;  // give an instantaneous force indepent of time, for things like jumping

    Vector2 preservedVelocity; // used for when the player is paused

    Vector2 prevMousePosition;

    bool mouseHold;
    bool mouseClick;

    float ropeCooldownTimer;

	// Use this for initialization
	void Start () {
        rb.position = startingPosition;
        prevMousePosition = Input.mousePosition;

        mouseClick = Input.GetMouseButtonDown(0);
        mouseHold = mouseClick;

    }
	
    void Update() {
        if (string.Equals(GameState.state, "gameplay"))
        {
            if (Input.GetMouseButtonDown(0))
                mouseClick = true;

            if (Input.GetMouseButtonUp(0))
                mouseClick = false;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        netForce = new Vector2(0, 0);
        netBurstForce = new Vector2(0, 0);
        decrementTimers();

        // if mouse is being held currently
        if (mouseHold)
        {
            // if mouse button is released at this frame
            if (!mouseClick)
            {
                mouseHold = false;
                lr.enabled = false;

                // give a boost if the user flicked their finger upon release
                netBurstForce += getBoostForce();

                // give a cooldown for when the bungee can be used again
                ropeCooldownTimer = ropeCooldown;

                // set player rotation back to neutral
                resetPlayerRotation();

                // turn off swinging animation
                animator.SetBool("isSwinging", false);

                // play backflip animation
                animator.SetBool("doBackflip", true);

                // ensure that the player is facing right
                sprite.flipX = false;


            }

            // otherwise add the forces due to the bungie and update the rope position
            // as well as update the orientation of the player sprite
            else
            {
                netForce += getBungeeForce();
                netForce += getDampingForce();

                lr.SetPosition(1, transform.position + (transform.up * sprite.size.y) + (new Vector3(rb.velocity.x, rb.velocity.y, 0) * Time.deltaTime));
                updatePlayerRotation();
            }
        }

        // if mouse isn't being held currently
        else
        {
            // check for mouse clicks, if it is clicked, the player will start swinging
            if (mouseClick & ropeCooldownTimer == 0f)
            {
                updateBungee();

                mouseHold = true;

                // set up line renderer
                lr.SetPosition(0, connectionPoint);
                lr.SetPosition(1, transform.position + (transform.up * sprite.size.y) + (new Vector3(rb.velocity.x, rb.velocity.y, 0) * Time.deltaTime));
                lr.enabled = true;

                // flip players sprite depending on where the rope is connected to relative to the player
                if ((connectionPoint - rb.position).y < 0)
                    sprite.flipX = true;

                // set up swinging animation
                animator.SetBool("doBackflip", false);
                animator.SetBool("isSwinging", true);
            }
        }

        prevMousePosition = Input.mousePosition;
        rb.AddForce(netForce * Time.deltaTime);
        rb.AddForce(netBurstForce);
	}

    Vector2 getBungeeForce()
    {
        float distanceFromEquilibrium = bungeeLength - (rb.position - connectionPoint).magnitude;

        // unlike a spring, a compressed bungee does not give a force
        if (distanceFromEquilibrium < 0)
        {
            return bungeeStiffness * (bungeeLength - (rb.position - connectionPoint).magnitude) * (rb.position - connectionPoint).normalized;
        }
        return new Vector2(0, 0);
    }

    Vector2 getDampingForce()
    {
        Vector2 bungeeForce = getBungeeForce();
        if (bungeeForce.magnitude != 0)
        {
            bungeeForce = bungeeForce.normalized;

            return -dampingConstant * Vector2.Dot(rb.velocity, bungeeForce) / Vector2.Dot(bungeeForce, bungeeForce) * bungeeForce;
        }
        return new Vector2(0, 0);
        //return -dampingConstant * rb.velocity;
    }

    Vector2 getBoostForce()
    {
        return ((Vector2)Input.mousePosition - prevMousePosition).normalized * boostScale;
    }

    void updateBungee()
    {
        connectionPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        bungeeLength = (rb.position - connectionPoint).magnitude;
    }

    void enableTrail()
    {
        trail.time = 0.2f;
    }

    void decrementTimers()
    {
        ropeCooldownTimer -= Time.deltaTime;
        if (ropeCooldownTimer < 0f)
            ropeCooldownTimer = 0f;
    }

    void updatePlayerRotation()
    {
        Vector2 direction = (connectionPoint - rb.position).normalized;
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        if (direction.x > 0)
            angle = -angle;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void resetPlayerRotation()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void StartPlayer()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(startingJump);
        ropeCooldownTimer = ropeCooldown;
        animator.SetBool("doBackflip", true);
    }
}
