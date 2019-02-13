using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{

    public int health;


    public float speed;

    private SpriteRenderer sr;
    private bool isAttacking = false;
    private bool isFacingRight = false;
    private Animator anim;
    private Player target;


    // Start is called before the first frame update
    void Start()
    {
        // get component references
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (!anim)
        {
            Debug.LogError("Animator not found on " + name);
        }
        if (!sr)
        {
            Debug.LogError("SpriteRenderer not found on " + name);
        }

        // make sure values are set in the inspector
        if (health == 0)
        {
            Debug.Log("Assign value in ispector for enemy " + name);
        }
    }

    private void Update()
    {
        
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaleFactor = transform.localScale;
        scaleFactor.x *= -1; // or -scaleFactor.x;
        transform.localScale = scaleFactor;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && !isAttacking)
        {
            target = other.GetComponent<Player>();
            if (target && !target.isDead)
            {
                isAttacking = true;
                anim.SetTrigger("Attack");
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && isAttacking)
        {
            ResetAttack();
        }
    }

    private void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Death();
        }
    }

    public void AttackTarget()
    {
        if (target)
        {
            target.TakeDamage();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void ResetAttack()
    {
        isAttacking = false;
        target = null;
    }
}
