using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public LineRenderer lr;
    public Animator animator;
    public AfterimageEffect afterimage;
    public BlipPlayer blip;

    public float bungeeStiffness;
    public float dampingConstant;
    public float boostScale;

    public float ropeCastSpeed;

    public Vector2 startingPosition;
    public Vector2 startingJump;

    Vector2 connectionPoint;
    Vector2 shootDirection;

    float ropeLength;
    Vector2 netForce;
    Vector2 netBurstForce;

    Vector2 prevMousePosition;

    bool mouseClick;    // is true only if the mouse is clicked on current frame
    bool mouseRelease;  // is true only if the mouse is released on current frame
    bool mouseHold;     // is true as long as the mouse is pressed, false otherwise


	void Start () {
        transform.position = startingPosition;
        prevMousePosition = Input.mousePosition;

        mouseClick = false;
        mouseRelease = false;
    }
	
    void Update() {
        if (GameState.state == "gameplay")
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

    void FixedUpdate () {
        netForce = new Vector2(0, 0);
        netBurstForce = new Vector2(0, 0);

        // if mouse was clicked on this frame
        if (mouseClick && PlayerState.isFreefall())
        {
            mouseClick = false;
            CastRope();
        }

        // if mouse was released on this frame
        if (mouseRelease)
        {
            mouseRelease = false;
            OnMouseRelease();
        }

        // if mouse is being held currently
        if (mouseHold)
        {
            OnMouseHold();
        }

        prevMousePosition = Input.mousePosition;

        // add forces as a function of time
        rb.AddForce(netForce * Time.fixedDeltaTime);
        // give an instantaneous force that is indepent of time, for actions such as jumping
        rb.AddForce(netBurstForce);
	}

    Vector2 getRopeHandPosition()
    {
       return transform.position + (transform.up * sprite.size.y) + (new Vector3(rb.velocity.x, rb.velocity.y, 0) * Time.deltaTime);
    }

    void OnMouseHold()
    {
        if (PlayerState.isSwinging())
        {
            // add the forces that the rope acts on the player and update the line renderer position
            // as well as update the orientation of the player sprite
            netForce += getRopeForce();
            netForce += getDampingForce();

            lr.SetPosition(1, getRopeHandPosition());
            SetPlayerRotation(connectionPoint - (Vector2) transform.position);
        }

        else if (PlayerState.isCasting())
        {
            RaycastHit2D hit = getCastStatus();
            lr.SetPosition(0, getRopeHandPosition() + ropeLength * shootDirection);
            lr.SetPosition(1, getRopeHandPosition());

            // if rope has hit a surface
            if (hit.collider != null && hit.collider.tag == "Platform")
            {
                AttachRope(hit.point, hit.distance);
            }
            else
            {
                ropeLength += ropeCastSpeed * Time.fixedDeltaTime;
            }
        }
    }

    void CastRope()
    {
        PlayerState.setToCasting();

        Vector2 origin = getRopeHandPosition();
        Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        shootDirection = clickPoint - origin;
        shootDirection = shootDirection.normalized;
        ropeLength = ropeCastSpeed * Time.fixedDeltaTime;

        // set up line renderer
        lr.SetPosition(0, origin + ropeLength * shootDirection);
        lr.SetPosition(1, origin);
        lr.enabled = true;

        // flip players sprite depending on the shoot direction
        if (shootDirection.y < 0)
            sprite.flipX = true;

        // rotate player to the direction they are shooting at
        SetPlayerRotation(shootDirection);

        // set up swinging animation
        animator.SetBool("doBackflip", false);
        animator.SetBool("isSwinging", true);
    }

    RaycastHit2D getCastStatus()
    {
        Vector2 origin = getRopeHandPosition();
        return Physics2D.Raycast(origin, shootDirection, ropeLength);
    }

    void AttachRope(Vector2 point, float length)
    {
        PlayerState.setToSwinging();
        blip.Play(point);

        connectionPoint = point;
        ropeLength = length;
    }

    void OnMouseRelease()
    {
        lr.enabled = false;

        // set player rotation back to neutral
        resetPlayerRotation();

        // turn off swinging animations
        animator.SetBool("isSwinging", false);

        // play backflip animation
        animator.SetBool("doBackflip", true);

        // ensure that the player is facing to the right
        sprite.flipX = false;

        // give a boost if the user flicked their finger upon release
        // players can only boost once per obstacle
        if (PlayerState.canBoost && PlayerState.isSwinging() && (Vector2)Input.mousePosition != prevMousePosition)
        {
            PlayerState.canBoost = false;
            netBurstForce += getBoostForce();
            afterimage.Play();
        }

        PlayerState.setToFreefall();
    }

    Vector2 getRopeForce()
    {
        Vector2 playerPosition = getRopeHandPosition();
        float distanceFromEquilibrium = ropeLength - (playerPosition - connectionPoint).magnitude;

        // unlike a spring, a compressed bungee does not give a force
        if (distanceFromEquilibrium < 0)
        {
            return bungeeStiffness * (ropeLength - (playerPosition - connectionPoint).magnitude) * (playerPosition - connectionPoint).normalized;
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
    }

    Vector2 getBoostForce()
    {
        return ((Vector2)Input.mousePosition - prevMousePosition).normalized * boostScale;
    }


    // Sets the Z-axis euler angle based on a 2D direction vector
    void SetPlayerRotation(Vector2 direction)
    {
        direction = direction.normalized;
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
        PlayerState.setToFreefall();
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(startingJump);
        animator.SetBool("doBackflip", true);
    }
}
