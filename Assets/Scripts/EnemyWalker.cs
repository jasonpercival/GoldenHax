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
        if (!isAttacking && target)
        {

            Player player = target.GetComponent<Player>();

            FaceTarget();
            if (!player.isDead)
            {
                Vector3 pos = Vector3.MoveTowards(rb.position, target.transform.position, speed * Time.deltaTime);
                rb.position = pos;

                if (rb.position != target.transform.position)
                {
                    anim.SetFloat("Movement", 1.0f);
                }
                else
                {
                    anim.SetFloat("Movement", 0.0f);
                }
            }
            else
            {
                anim.SetFloat("Movement", 0.0f);
            }
        }
    }

    public override void Attack()
    {
        FaceTarget();
        isAttacking = true;
        anim.SetTrigger("Attack");
        rb.velocity = Vector3.zero;
        anim.SetFloat("Movement", Mathf.Abs(rb.velocity.x));
    }

    // Constantly attack player if they are within enemy collider
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isAttacking)
        {
            Player player = other.GetComponent<Player>();
            if (!player.isDead)
            {
                Attack();
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
            if (!player.isDead)
            {
                player.TakeDamage();
            }
        }
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        anim.SetTrigger("Hit");
    }

}
