using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public LineRenderer lr;
    public TrailRenderer trail;
    public Animator animator;
    public AfterimageEffect afterimage;

    public float bungeeStiffness;
    public float dampingConstant;
    public float boostScale;
    public float ropeCooldown;

    public Vector2 startingPosition;
    public Vector2 startingJump;

    Vector2 connectionPoint;
    float ropeLength;
    Vector2 netForce;
    Vector2 netBurstForce;

    Vector2 preservedVelocity; // used for when the player is paused

    Vector2 prevMousePosition;

    bool mouseClick;    // is true only if the mouse is clicked on current frame
    bool mouseRelease;  // is true only if the mouse is released on current frame
    bool mouseHold;     // is true as long as the mouse is pressed, false otherwise


    float ropeCooldownTimer;

	void Start () {
        rb.position = startingPosition;
        prevMousePosition = Input.mousePosition;

        mouseClick = false;
        mouseRelease = false;
    }
	
    void Update() {
        if (string.Equals(GameState.state, "gameplay"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseClick = true;
                mouseHold = true;
            }

            if (Input.GetMouseButtonUp(0) & mouseHold)
            {
                mouseRelease = true;
                mouseHold = false;
            }
        }
    }

	void FixedUpdate () {
        netForce = new Vector2(0, 0);
        netBurstForce = new Vector2(0, 0);
        decrementTimers();

        // if mouse was clicked on this frame
        if (mouseClick)
        {
            mouseClick = false;
            ShootRope();
        }

        // if mouse was released on this frame
        if (mouseRelease)
        {
            mouseRelease = false;
            ReleaseRope();
        }

        // if mouse is being held currently
        if (mouseHold)
        {
            // add the forces that the rope acts on the player and update the line renderer position
            // as well as update the orientation of the player sprite
            netForce += getRopeForce();
            netForce += getDampingForce();

            lr.SetPosition(1, transform.position + (transform.up * sprite.size.y) + (new Vector3(rb.velocity.x, rb.velocity.y, 0) * Time.deltaTime));
            updatePlayerRotation();
        }

        prevMousePosition = Input.mousePosition;

        // add forces as a function of time
        rb.AddForce(netForce * Time.fixedDeltaTime);

        // give an instantaneous force indepent of time, for things like jumping
        rb.AddForce(netBurstForce);
	}

    void ShootRope()
    {
        updateRope();

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

    void ReleaseRope()
    {
        lr.enabled = false;

        // give a boost if the user flicked their finger upon release
        // players can only boost once per obstacle
        if ((Vector2)Input.mousePosition != prevMousePosition & GameState.canBoost)
        {
            GameState.canBoost = false;
            netBurstForce += getBoostForce();
            afterimage.Play();
        }

        // give a cooldown for when the rope swing can be used again
        ropeCooldownTimer = ropeCooldown;

        // set player rotation back to neutral
        resetPlayerRotation();

        // turn off swinging animation
        animator.SetBool("isSwinging", false);

        // play backflip animation
        animator.SetBool("doBackflip", true);

        // ensure that the player is facing to the right
        sprite.flipX = false;
    }

    Vector2 getRopeForce()
    {
        float distanceFromEquilibrium = ropeLength - (rb.position - connectionPoint).magnitude;

        // unlike a spring, a compressed bungee does not give a force
        if (distanceFromEquilibrium < 0)
        {
            return bungeeStiffness * (ropeLength - (rb.position - connectionPoint).magnitude) * (rb.position - connectionPoint).normalized;
        }
        return new Vector2(0, 0);
    }

    Vector2 getDampingForce()
    {
        Vector2 RopeForce = getRopeForce();
        if (RopeForce.magnitude != 0)
        {
            RopeForce = RopeForce.normalized;

            return -dampingConstant * Vector2.Dot(rb.velocity, RopeForce) / Vector2.Dot(RopeForce, RopeForce) * RopeForce;
        }
        return new Vector2(0, 0);
        //return -dampingConstant * rb.velocity;
    }

    Vector2 getBoostForce()
    {
        return ((Vector2)Input.mousePosition - prevMousePosition).normalized * boostScale;
    }

    void updateRope()
    {
        connectionPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ropeLength = (rb.position - connectionPoint).magnitude;
    }

    void enableTrail()
    {
        trail.time = 0.2f;
    }

    void decrementTimers()
    {
        ropeCooldownTimer -= Time.fixedDeltaTime;
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
