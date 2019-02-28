using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : Enemy
{
    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!isAttacking)
        {
            // Move in the direction the enemy is facing
            if (isFacingRight)
            {
                rb.velocity = new Vector3(speed, 0, 0);
            }
            else
            {
                rb.velocity = new Vector3(-speed, 0, 0);
            }

            anim.SetFloat("Movement", Mathf.Abs(rb.velocity.x));
        }
    }

    // Constantly attack player if they are within enemy collider
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isAttacking)
        {
            FaceTarget();
            Player player = other.GetComponent<Player>();
            if (!player.isDead)
            {
                isAttacking = true;
                anim.SetTrigger("Attack");
                rb.velocity = Vector3.zero;
                anim.SetFloat("Movement", Mathf.Abs(rb.velocity.x));
            }
        }
    }

    // Stop attack if player is not in range
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isAttacking)
        {
            ResetAttack();
        }
    }

    // Walk the other direction if we hit a wall
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("EnemyWalker: Collision with " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Boundary"))
        {
            Flip();
        }
    }

    // animation end event to reset attack
    public void ResetAttack()
    {
        isAttacking = false;
    }

    // animation event for attacking
    public void AttackTarget()
    {
        if (target)
        {
            Player player = target.GetComponent<Player>();
            player.TakeDamage();
        }
    }

}
