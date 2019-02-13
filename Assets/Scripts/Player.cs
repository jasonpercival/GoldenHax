using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float MinPlayerX;
    public float MaxPlayerX;
    public AudioSource audioSrc;
    public AudioClip deathClip, damageClip, attackClip;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Animator anim;
    private SpriteRenderer sr;

    private bool isFacingRight = true;
    private bool isAttacking = false;
    private bool isHit = false;
    public bool isDead = false;

    float horizontalMovement = 0.0f;
    float verticalMovement = 0.0f;

    // TODO: Move this game state to the game manager when it's ready
    public int healthMax = 6;
    public int health = 6;
    public int potionsMax = 6;
    public int potions = 1;

    void Start()
    {
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
                moveDirection.z = 0.0f;
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

        if ((isFacingRight && horizontalMovement < 0) || (!isFacingRight && horizontalMovement > 0))
            Flip();

        if (anim)
        {
            anim.SetBool("Grounded", controller.isGrounded);
            anim.SetFloat("HorizontalMovement", Mathf.Abs(horizontalMovement));
            anim.SetFloat("VerticalMovement", verticalMovement);
        }

        // Move the player only if jumping or not attackng while grounded and not hit
        if (!controller.isGrounded || (controller.isGrounded && !isAttacking) && !isHit)
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
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
            StartCoroutine(FlashPlayer(Color.gray));
        }
    }

    // Make the player sprite flash temporarily with a given color
    IEnumerator FlashPlayer(Color color, float duration = 5.0f)
    {

        for (int i = 0; i < duration; i++)
        {
            sr.color = color;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Magic()
    {
        if (potions > 0)
        {
            anim.SetTrigger("Casting");
            potions = 0;
        }

    }

    public void TakeDamage()
    {
        isHit = true;
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
        isHit = false;
        Debug.Log("Hit complete!");
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
        StartCoroutine(FlashPlayer(Color.gray, 10.0f));

        // Respawn the player
        StartCoroutine(Respawn());
    }

    // Respawns the player from death after a timeout
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5.0f);

        isDead = false;
        isHit = false;
        health = healthMax;
        anim.SetBool("IsDead", isDead);
    }

    private void LateUpdate()
    {
        // clamp the player into the play area
        Vector3 targetPosition = transform.position;
        targetPosition.x = Mathf.Clamp(targetPosition.x, MinPlayerX, MaxPlayerX);
        transform.position = targetPosition;
    }
}
