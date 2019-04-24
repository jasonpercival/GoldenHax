using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodger : Enemy
{

    public GameObject collectibleToDrop;
    private bool isDodging;
    private Vector3 targetPos;

    public override void Start()
    {
        base.Start();
        isDodging = false;
    }

    private void Update()
    {
        if (Time.time > timeSinceLastFire + attackRate)
        {
            Attack();
            timeSinceLastFire = Time.time;
        }
        
        if (isDodging)
        {
            Dodge();
        }
    }

    private void Dodge()
    {
        // move left or right away from player instead of attacking
        Vector3 pos = Vector3.MoveTowards(rb.position, targetPos, speed * Time.deltaTime);
        rb.position = pos;

        if (rb.position != target.transform.position)
        {
            anim.SetFloat("Movement", 1.0f);
        }
        else
        {
            anim.SetFloat("Movement", 0.0f);
            FaceTarget();
            isDodging = false;
        }
    }

    public override void Attack()
    {
        // enemy is passive and only avoids player
        isDodging = true;

        float horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        float xpos = Random.Range(1, horizontalExtent * 2.0f);
        float ypos = transform.position.y * Random.Range(-1, 1);
        float zpos = transform.position.z;
        targetPos = new Vector3(xpos, ypos, zpos);

    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        timeSinceLastFire = Time.time;
        anim.SetFloat("Movement", 0);
        anim.SetTrigger("Hit");
        isDodging = false;
        rb.AddForce(new Vector3(-1 * Random.Range(4, 6), 1 * Random.Range(5, 8)), ForceMode.Impulse);

        // spawn object
        if (collectibleToDrop)
        {
            var temp = Instantiate(collectibleToDrop, transform.position, Quaternion.identity);
            Rigidbody rb = temp.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(-1 * Random.Range(2,3), 1 * Random.Range(8,10)), ForceMode.Impulse);
        }
    }


}
