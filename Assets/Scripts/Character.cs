using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    // configurable speed values
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 3.0f;
    public float jumpForce = 10.0f;

    // game state
    public int maxHealth = 6;
    public int currentHealth = 6;
    public int maxPotions = 6;
    public int currentPotions = 1;
    public bool isDead = false;
    public bool isHit = false;

    // references to other components
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private EdgeCollider2D worldCollider;
    private AudioSource audioSource;

    // Sound FX
    public AudioClip attackClip;
    public AudioClip deathClip;
    public AudioClip damageClip;

    // runtime state variables
    private float horizontalMovement = 0.0f;
    private float verticalMovement = 0.0f;
    private bool facingRight = true;
    private bool isJumping = false;
    private Vector3 jumpPosition;       // original Y position before jump started

    // attacking state
    private bool isAttacking = false;   
    private float attackSpeed = 0.5f;   // attack rate speed limit 

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

        worldCollider = GameObject.Find("Boundary").GetComponentInChildren<EdgeCollider2D>();
        if (!worldCollider)
        {
            Debug.LogError("World Edge Collider not found in " + name);
        }
        
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogError("AudioSource not found on " + name);
        }

        rb.gravityScale = 0.0f;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.freezeRotation = true;
    }

    void Update()
    {      
        // Adjust sprite sorting order to ensure player is in front/behind other objects based on the current y position
        if (!isJumping)
        {
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        }

        // Prevent movement while attacking
        if (isAttacking || isDead || isHit)
        {
            horizontalMovement = 0.0f;
            verticalMovement = 0.0f;
        }
        else
        {
            // Read keyboard/controller input for movement
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");
        }

        if (!isAttacking && !isHit)
        {
            // Jump
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                Jump();
            }

            // Attack
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
            }

            // Cast Magic 
            if (Input.GetButtonDown("Fire2") && !isJumping)
            {
                Magic();
            }
        }

        // Flip the player sprite if facing to the left/right
        if ((facingRight && horizontalMovement < 0) || (!facingRight && horizontalMovement > 0))
        {
            sr.flipX = facingRight;
            facingRight = !facingRight;
        }

        // Pass movement values to animator to handle animation transitions
        if (animator)
        {
            animator.SetFloat("HorizontalMovement", Mathf.Abs(horizontalMovement));
            animator.SetFloat("VerticalMovement", verticalMovement);
        }

        // update the player's position
        if (!isJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * horizontalSpeed, verticalMovement * verticalSpeed);
        }
        else
        {
            rb.velocity = new Vector2(horizontalMovement * horizontalSpeed * 0.75f, rb.velocity.y);

            // don't allow player to fall below their original vertical position
            if (transform.position.y < jumpPosition.y || rb.velocity.y == 0.0f)
            {
                isJumping = false;
                rb.gravityScale = 0.0f;
                animator.SetBool("IsJumping", false);
                Physics2D.IgnoreCollision(playerCollider, worldCollider, false);
            }
        }

        // Check for death
        if (currentHealth < 1 && !isDead)
        {
            Death();
        }

    }

    // Resets to idle state after attack is finished
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackSpeed);
        isAttacking = false;
    }

    // Attack animation event handler mid-slash
    private void Attack()
    {
        isAttacking = true;
        StartCoroutine(ResetAttack());
        animator.SetTrigger("Attack");

        // Play sword attack sound
        audioSource.PlayOneShot(attackClip);

        // TODO: check for enemy collisions and apply damage
    }

    private void Magic()
    {
        isAttacking = true;
        StartCoroutine(ResetAttack());
        animator.SetTrigger("Casting");
    }

    private void Jump()
    {
        // save original position of player before jump
        isJumping = true;
        jumpPosition = transform.position;

        // turn gravity back on
        rb.gravityScale = 1.7f;
        
        // Applies a force in UP direction
        animator.SetBool("IsJumping", true);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        Physics2D.IgnoreCollision(playerCollider, worldCollider, true);
    }

    // Death animation
    public void Death()
    {
        // Play death sound clip
        if (deathClip)
        {
            audioSource.PlayOneShot(deathClip);
        }

        // Start death animation
        isDead = true;
        currentHealth = 0;
        animator.SetBool("IsDead", isDead);
        StartCoroutine(FlashPlayer(10.0f));

        // Respawn the player
        StartCoroutine(Respawn());
    }

    public void TakeDamage()
    {
        isHit = true;
        animator.SetTrigger("Damage");

        if (damageClip)
        {
            audioSource.PlayOneShot(damageClip);
        }

        currentHealth--;
        if (currentHealth < 0)
        {
            Death();
        }
    }

    public void TakeDamageComplete()
    {
        isHit = false;
        Debug.Log("Hit complete!");
    }


    // Respawns the player after death after a timeout
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5.0f);
        isDead = false;
        isHit = false;
        currentHealth = maxHealth;
        animator.SetBool("IsDead", isDead);
     }

    // Feedback to player when item picked up
    public void PickupItem()
    {
        StartCoroutine(FlashPlayer());
    }

    // Make the player sprite flash temporarily
    IEnumerator FlashPlayer(float duration = 5.0f)
    {
        for (int i = 0; i < duration; i++)
        {
            sr.color = Color.gray;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
