using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // default speed values
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 3.0f;

    // references to components
    private Animator animator;
    private SpriteRenderer sr; 
    private Rigidbody2D rb;

    private float horizontalMovement = 0.0f;
    private float verticalMovement = 0.0f;
    private bool facingLeft = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0.0f;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // read keyboard/controller input for movement
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // flip the player sprite if facing to the left
        if (horizontalMovement != 0)
        {
            facingLeft = (horizontalMovement < 0);
            sr.flipX = facingLeft;
        }

        // pass movement values to animator to handle animation transitions
        if (animator)
        {
            animator.SetFloat("horizontalMovement", horizontalMovement);
            animator.SetFloat("verticalMovement", verticalMovement);
        }

        // update the player's position
        rb.velocity = new Vector2(horizontalMovement * horizontalSpeed, verticalMovement * verticalSpeed);
    }
}
