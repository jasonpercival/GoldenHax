using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isAttacking = false;
    private Animator anim;
    private Player target;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (!anim)
        {
            Debug.LogWarning("Animator not found on " + name);
        }
        if (!sr)
        {
            Debug.LogWarning("Animator not found on " + name);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (anim)
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && isAttacking)
        {
            target = null;
        }
    }

    public void AttackTarget()
    {
        if (target)
        {
            target.TakeDamage();
        }
    }

    public void ResetAttack()
    {
        isAttacking = false;
        target = null;
    }
}
