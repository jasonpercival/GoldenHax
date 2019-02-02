using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    // default speed values
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 3.0f;
    public float jumpForce = 10.0f;

    public int maxHealth = 6;
    public int currentHealth = 6;

    public int maxPotions;
    public int currentPotions = 1;



    // references to components
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private AudioSource audioSource;
    public AudioClip attackClip;

    // runtime state variables
    private float horizontalMovement = 0.0f;
    private float verticalMovement = 0.0f;
    private bool facingRight = true;

    private bool isJumping = false;
    private Vector3 jumpPosition;         // original Y position before jump started

    // attacking state
    private bool isAttacking = false;
    private float attackSpeed = 0.3f;   // attack rate speed limit 
    private float nextAttack;           // next time attack is allowed

    private void Start()
    {
        // Get reference to the animator component
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("Animator not found on " + name);
        }

        // Get reference to the sprite renderer
        sr = GetComponent<SpriteRenderer>();
        if (!sr)
        {
            Debug.LogError("SpriteRenderer not found on " + name);
        }

        // Get reference to the rigibody components
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError("Rigidbody2D not found on " + name);
        }

        playerCollider = GetComponent<BoxCollider2D>();
        if (!playerCollider)
        {
            Debug.LogError("BoxCollider2D not found on " + name);
        }


        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogError("AudioSource not found on " + name);
        }


        rb.gravityScale = 0.0f;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.freezeRotation = true;
        nextAttack = Time.time;
    }

    void Update()
    {

        // Adjust sorting order to make sure player is in front/behind other objects based on current y position
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;

        // read keyboard/controller input for movement
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // Attack
        if (!isAttacking && Input.GetButtonDown("Fire1") && nextAttack < Time.time)
        {
            isAttacking = true;
            animator.SetTrigger("attack");
            Debug.Log("Attack Start");
            nextAttack = Time.time + attackSpeed;

        }

        // prevent movement while attacking
        if (isAttacking)
        {
            horizontalMovement = 0.0f;
            verticalMovement = 0.0f;
        }

        // Magic 
        if (Input.GetButtonDown("Fire2"))
        {
            Magic();
        }

        // Jump
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            Jump();
        }

        // flip the player sprite if facing to the left/right
        if ((facingRight && horizontalMovement < 0) || (!facingRight && horizontalMovement > 0))
        {
            sr.flipX = facingRight;
            facingRight = !facingRight;
        }

        // pass movement values to animator to handle animation transitions
        if (animator)
        {
            animator.SetFloat("horizontalMovement", Mathf.Abs(horizontalMovement));
            animator.SetFloat("verticalMovement", verticalMovement);
        }

        // update the player's position
        if (!isJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * horizontalSpeed, verticalMovement * verticalSpeed);
        }
        //else
        //{
        //    rb.velocity = new Vector2(horizontalMovement * horizontalSpeed * 0.75f, rb.velocity.y);
        //    // check if the player returned to the original ground position after jumping and reset player
        //    if (transform.position.y < jumpPosition.y)
        //    {

        //        isJumping = false;
        //        Vector3 position = transform.position;
        //        position.y = jumpPosition.y;
        //        transform.position = position;
        //        rb.gravityScale = 0.0f;
        //        animator.SetBool("jumping", isJumping);
        //        playerCollider.enabled = true;
        //    }
        //}


    }

    // Attack animation event handler mid-slash
    private void Attack()
    {
        Debug.Log("Attack Stop");

        // Play sword attack sound
        audioSource.PlayOneShot(attackClip);

        // TODO: check for enemy collision
        isAttacking = false;
    }

    private void Magic()
    {
        Debug.Log("Use Magic Potion");
    }

    private void Jump()
    {
        Debug.Log("Jump");
        //// save original position of player before jump
        //jumpPosition = transform.position;
        //verticalMovement = 0.0f;

        //// Applies a force in UP direction
        //isJumping = true;
        //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //animator.SetBool("jumping", isJumping);

        //// turn gravity back on
        //rb.gravityScale = 1.7f;

        //// turn off collider while jumping
        //playerCollider.enabled = false;

    }


}
