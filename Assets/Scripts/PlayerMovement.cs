using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public Transform tf;
    public Rigidbody2D rb;
    public LineRenderer lr;
    public TrailRenderer trail;

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

    Vector2 prevMousePosition;

    bool mouseHold;
    bool mouseClick;

    float ropeCooldownTimer;

	// Use this for initialization
	void Start () {
        rb.position = startingPosition;
        rb.AddForce(startingJump);
        prevMousePosition = Input.mousePosition;
        ropeCooldownTimer = ropeCooldown;

        mouseClick = Input.GetMouseButtonDown(0);
        mouseHold = mouseClick;

    }
	
    void Update() {
        if (Input.GetMouseButtonDown(0))
            mouseClick = true;

        if (Input.GetMouseButtonUp(0))
            mouseClick = false;
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
            }

            // otherwise add the forces due to the bungie and update the rope position
            else
            {
                netForce += getBungeeForce();
                netForce += getDampingForce();

                lr.SetPosition(1, transform.position);
            }
        }

        // if mouse isn't being held currently
        else
        {
            // check for mouse clicks
            if (mouseClick & ropeCooldownTimer == 0f)
            {
                updateBungee();

                mouseHold = true;

                lr.SetPosition(0, connectionPoint);
                lr.SetPosition(1, transform.position);
                lr.enabled = true;
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
}
