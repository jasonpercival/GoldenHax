using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;          // current health
    public float speed;         // movement speed
    public float attackRate;    // how fast can the enemy attack

    protected SpriteRenderer sr;
    protected float timeSinceLastFire;
    protected bool isAttacking = false;
    protected bool isFacingRight = false;

    protected Animator anim;
    protected GameObject target = null;
    protected Rigidbody rb;

    // Start is called before the first frame update
    public virtual void Start()
    {
        // set component references
        sr = GetComponent<SpriteRenderer>();
        if (!sr)
        {
            Debug.LogError("SpriteRenderer not found on " + name);
        }

        anim = GetComponent<Animator>();
        if (!anim)
        {
            Debug.LogError("Animator not found on " + name);
        }

        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogError("RigidBody not found on " + name);
        }

        // make sure values are set in the inspector
        if (health <= 0)
        {
            Debug.Log("Assign health for enemy " + name);
        }

        if (speed <= 0)
        {
            speed = 1.0f;
            Debug.Log("Speed value not set in inspector for " + name);
        }

        if (attackRate <= 0)
        {
            attackRate = 2.0f;
            Debug.LogWarning("AttackRate not set. Defaulting to " + attackRate);
        }

        if (!target)
        {
            target = GameObject.FindWithTag("Player");
        }

        rb.constraints = RigidbodyConstraints.FreezeRotation;

    }

    protected void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaleFactor = transform.localScale;
        scaleFactor.x *= -1; // or -scaleFactor.x;
        transform.localScale = scaleFactor;
    }

    // check for damage by player weapon
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            //Debug.Log("Enemy: Collision with " + other.gameObject.name + ", Tag: " + other.gameObject.tag);
            TakeDamage();
        }
    }

    // flip the sprite to face the target object
    protected void FaceTarget()
    {
        if (!target) return;

        Vector3 difference = target.transform.position - transform.position;
        if (difference.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (difference.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    public virtual void TakeDamage()
    {
        health--;
        StartCoroutine(FlashSprite(Color.red));
        if (health <= 0)
        {
            health = 0;
            Death();
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

    public virtual void Death()
    {
        Destroy(gameObject, 1.0f);
    }


}
