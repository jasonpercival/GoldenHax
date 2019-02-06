using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool isAttacking = false;
    private Animator anim;
    private Character target;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (!anim)
        {
            Debug.LogWarning("Animator not found on " + name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (anim)
        {
            if (collision.tag == "Player" && !isAttacking)
            {
                target = collision.GetComponent<Character>();
                if (target && !target.isDead)
                {
                    isAttacking = true;
                    anim.SetTrigger("Attack");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isAttacking)
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
