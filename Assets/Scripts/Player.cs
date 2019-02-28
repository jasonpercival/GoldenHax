using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // player options
    public float speed;
    public float jumpSpeed;
    public float gravity = 20.0f;
    public float MinPlayerX;            // player min X movement constraint
    public float MaxPlayerX;            // player max X movement constraint

    // audio references
    public AudioSource audioSrc;
    public AudioClip deathClip, damageClip, attackClip;

    // player state
    public bool isDead = false;
    private bool isFacingRight = true;
    private bool isAttacking = false;
    private bool isFrozen = false;

    // component references
    private CharacterController controller;
    private Animator anim;
    private SpriteRenderer sr;

    // movement variables
    private float horizontalMovement = 0.0f;
    private float verticalMovement = 0.0f;
    private Vector3 moveDirection = Vector3.zero;

    // TODO: game state - move this to the game manager when it's ready
    public int healthMax = 6;
    public int health = 6;
    public int potionsMax = 6;
    public int potions = 1;
    public int lives = 2;

    void Start()
    {
        tag = "Player";

        controller = GetComponent<CharacterController>();
        if (!controller)
        {
            Debug.LogError("Unable to get Character Controller component on " + name);
        }

        sr = GetComponent<SpriteRenderer>();
        if (!sr)
        {
            Debug.LogError("Unable to get Character Controller component on " + name);
        }

        anim = GetComponent<Animator>();
        if (!anim)
        {
            Debug.LogError("Unable to get Character Controller component on " + name);
        }

        if (!audioSrc)
        {
            audioSrc = GameObject.FindWithTag("GameController").GetComponent<AudioSource>();
            if (!audioSrc)
            {
                Debug.LogError("Unable to get audio source on " + name);
            }
        }

        // Check if speed variable was set in the inspector
        if (speed <= 0 || speed > 10.0f)
        {
            // Assign a default value
            speed = 6.0f;
            Debug.LogWarning("Speed not set on " + name + ". Defaulting to " + speed);
        }

        // Check if jumpSpeed variable was set in the inspector
        if (jumpSpeed <= 0 || jumpSpeed > 10.0f)
        {
            // Assign a default value
            jumpSpeed = 8.0f;
            Debug.LogWarning("JumpForce not set on " + name + ". Defaulting to " + jumpSpeed);
        }

        // Check movement constraints
        if (MinPlayerX == 0 || MaxPlayerX == 0)
        {
            Debug.LogWarning("Player MinX/MaxX movement constraints not set.");
        }


    }

    void Update()
    {
        // Movement controls
        if (controller.isGrounded)
        {
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector3(horizontalMovement, -1.0f, verticalMovement);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            // Jump allowed if grounded 
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
                moveDirection.z = 0.0f;         // only allow jumping straight up and down
            }

            // Cast Magic 
            if (Input.GetButtonDown("Fire2"))
            {
                Magic();
            }

        }
        else
        {
            // Apply gravity otherwise since we are jumping
            moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        }

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
            StartCoroutine(ResetAttack());
            if (audioSrc)
            {
                audioSrc.PlayOneShot(attackClip);
            }
        }

        // Flip the sprite if moving in different direction
        if (!isDead)
        {
            if ((isFacingRight && horizontalMovement < 0) ||
                (!isFacingRight && horizontalMovement > 0))
            {
                Flip();
            }
        }

        // Update animator with movement values
        if (anim)
        {
            anim.SetBool("Grounded", controller.isGrounded);
            anim.SetFloat("HorizontalMovement", Mathf.Abs(horizontalMovement));
            anim.SetFloat("VerticalMovement", verticalMovement);
        }

        // Move the player only if jumping or not attackng while grounded and not frozen
        if (!controller.isGrounded || (controller.isGrounded && !isAttacking) && !isFrozen && !isDead)
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        // clamp the player into the play area
        Vector3 targetPosition = transform.position;
        targetPosition.x = Mathf.Clamp(targetPosition.x, MinPlayerX, MaxPlayerX);
        transform.position = targetPosition;
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking = false;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaleFactor = transform.localScale;
        scaleFactor.x *= -1; // or -scaleFactor.x;
        transform.localScale = scaleFactor;
    }

    public void AddHealth(int amount)
    {
        health += amount;
        if (health > healthMax) health = healthMax;
    }

    public void AddPotion(int amount)
    {
        potions += amount;
        if (potions > potionsMax) potions = potionsMax;
    }

    // Feedback to player when item picked up
    public void CollectItem(int healthBonus, int potionBonus)
    {
        if (healthBonus > 0 || potionBonus > 0)
        {
            AddHealth(healthBonus);
            AddPotion(potionBonus);
            StartCoroutine(FlashSprite(Color.gray));
        }
    }

    // Make the player sprite flash temporarily with a given color
    IEnumerator FlashSprite(Color color, float duration = 5.0f)
    {
        for (int i = 0; i < duration; i++)
        {
            sr.color = color;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // cast magic potion
    private void Magic()
    {
        if (potions > 0)
        {
            isFrozen = true;
            anim.SetTrigger("Casting");
            StartCoroutine(ResetFrozen(5.0f));
            potions = 0;
            // TODO: implement magic animation effect and knockdown all enemies
        }
    }

    IEnumerator ResetFrozen(float time)
    {
        yield return new WaitForSeconds(time);
        isFrozen = false;
    }

    public void TakeDamage()
    {
        isFrozen = true;
        StartCoroutine(ResetFrozen(1.0f));
        anim.SetTrigger("Damage");

        if (damageClip)
        {
            audioSrc.PlayOneShot(damageClip);
        }

        health--;
        if (health <= 0)
        {
            Death();
        }
    }

    public void TakeDamageComplete()
    {
        isFrozen = false;
    }

    // Death animation
    public void Death()
    {
        // Play death sound clip
        if (deathClip)
        {
            audioSrc.PlayOneShot(deathClip);
        }

        // Start death animation
        isDead = true;
        anim.SetBool("IsDead", isDead);
        StartCoroutine(FlashSprite(Color.gray, 10.0f));

        lives--;
        if (lives > 0)
        {
            // Respawn the player
            StartCoroutine(Respawn());
        }
        else
        {
            StartCoroutine(Restart());
        }
    }

    // Respawns the player from death after a timeout
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5.0f);

        isDead = false;
        isFrozen = false;
        health = healthMax;
        anim.SetBool("IsDead", isDead);
    }

    // Restart the game by reloading the scene
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
