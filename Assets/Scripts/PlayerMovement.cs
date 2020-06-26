﻿using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour {

    SpriteRenderer sprite;
    Rigidbody2D rb;
    LineRenderer lr;
    Animator animator;
    AfterimageEffect afterimage;

    public BlipPlayer blip;
    public AudioManager audioManager;

    public float bungeeStiffness;
    public float dampingConstant;
    public float boostScale;

    public float ropeCastSpeed;

    Vector2 connectionPoint;
    Vector2 shootDirection;

    float ropeLength;

    Vector2 netForce;
    Vector2 netBurstForce;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        afterimage = GetComponent<AfterimageEffect>();

    }
    void FixedUpdate () {
        netForce.Set(0, 0);
        netBurstForce.Set(0, 0);

        // if mouse was clicked on this frame
        if (PlayerState.mouseClick && PlayerState.IsFreefall())
        {
            PlayerState.mouseClick = false;
            CastRope();
        }

        // if mouse was released on this frame
        if (PlayerState.mouseRelease)
        {
            PlayerState.mouseRelease = false;
            OnMouseRelease();
        }

        // if mouse is being held currently
        if (PlayerState.mouseHold)
        {
            OnMouseHold();
        }

        // add forces as a function of time
        rb.AddForce(netForce * Time.fixedDeltaTime);
        // give an instantaneous force that is indepent of time, for actions such as jumping
        rb.AddForce(netBurstForce);
	}

    Vector2 GetRopeHandPosition()
    {
       return transform.position + (transform.up * (sprite.size.y / 2.0f)) + (new Vector3(rb.velocity.x, rb.velocity.y, 0) * Time.deltaTime);
    }

    void OnMouseHold()
    {
        if (PlayerState.IsSwinging())
        {
            // add the forces that the rope acts on the player and update the line renderer position
            // as well as update the orientation of the player sprite
            netForce += GetRopeForce();
            netForce += GetDampingForce();

            lr.SetPosition(1, GetRopeHandPosition());
            SetPlayerRotation(connectionPoint - (Vector2) transform.position);
        }

        else if (PlayerState.IsCasting())
        {
            lr.SetPosition(0, GetRopeHandPosition() + ropeLength * shootDirection);
            lr.SetPosition(1, GetRopeHandPosition());

            RaycastHit2D hit = GetCastStatus();
            // if rope has hit a surface
            if (hit.collider != null && hit.collider.CompareTag("Platform"))
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
        PlayerState.SetToCasting();

        Vector2 origin = GetRopeHandPosition();

        // determing the angle of the shoot direction
        // relative to finger press position on the screen
        if (GameState.RelativeMousePositionX() > 0)
            shootDirection = AngleToVectorD(Mathf.Lerp(75f , 50f, GameState.RelativeMousePositionX()));
        else
            shootDirection = AngleToVectorD(Mathf.Lerp(95f, 75f, GameState.RelativeMousePositionX() + 1));

        // shoot downward when player taps on bottom half of screen
        if (GameState.RelativeMousePositionY() < 0)
            shootDirection.y = -shootDirection.y;

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
        animator.SetBool("isSwinging", true);

        audioManager.Play("RopeShoot");
    }

    RaycastHit2D GetCastStatus()
    {
        Vector2 origin = GetRopeHandPosition();
        return Physics2D.Raycast(origin, shootDirection, ropeLength);
    }

    void AttachRope(Vector2 point, float length)
    {
        PlayerState.SetToSwinging();
        blip.Play(point);

        connectionPoint = point;
        ropeLength = length;
        lr.SetPosition(0, point);
    }

    void OnMouseRelease()
    {
        lr.enabled = false;

        // set player rotation back to neutral
        ResetPlayerRotation();

        // turn off swinging animations
        animator.SetBool("isSwinging", false);

        // play backflip animation
        animator.SetTrigger("doBackflip");

        // ensure that the player is facing to the right
        sprite.flipX = false;

        // give a boost if the user quickly flicked their finger upon release
        // players can only boost once per obstacle
        if (PlayerState.BoostInput())
        {
            PlayerState.canBoost = false;
            netBurstForce += GetBoostForce(GameState.cursorVelocity);
            afterimage.Play();
            audioManager.Play("BigLeap");
        }

        PlayerState.SetToFreefall();
    }

    Vector2 GetRopeForce()
    {
        Vector2 playerPosition = GetRopeHandPosition();
        float distanceFromEquilibrium = ropeLength - (playerPosition - connectionPoint).magnitude;

        // unlike a spring, a compressed rope does not give a force
        if (distanceFromEquilibrium < 0)
        {
            return bungeeStiffness * distanceFromEquilibrium * (playerPosition - connectionPoint).normalized;
        }
        return new Vector2(0, 0);
    }

    Vector2 GetDampingForce()
    {
        Vector2 RopeForce = GetRopeForce();

        if (RopeForce.magnitude == 0)
            return new Vector2(0, 0);

        RopeForce.Normalize();
        return -dampingConstant * Vector2.Dot(rb.velocity, RopeForce) / Vector2.Dot(RopeForce, RopeForce) * RopeForce;
    }

    Vector2 GetBoostForce(Vector2 boostDirection)
    {
        return boostDirection.normalized * boostScale;
    }


    // Sets the Z-axis euler angle based on a 2D direction vector
    void SetPlayerRotation(Vector2 direction)
    {
        direction.Normalize();
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        if (direction.x > 0)
            angle = -angle;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void ResetPlayerRotation()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // returns a 2D unit vector pointing to the specified angle in degrees
    Vector2 AngleToVectorD(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }
}
