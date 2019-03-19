using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{
    public Rigidbody projectile;            // What projectile to spawn 
    public Transform projectileSpawnPoint;  // Where to spawn projectile
    
    public override void Start()
    {
        base.Start();

        // Check if 'projectile' variable was set in the inspector
        if (!projectile)
        {
            Debug.LogError("Projectile not found on " + name);
        }

        // Check if 'projectileSpawnPoint' variable was set in the inspector
        if (!projectileSpawnPoint)
        {
            Debug.LogError("ProjectileSpawnPoint not found on " + name);
        }

        FaceTarget();
    }

    private void Update()
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

    public override void Attack()
    {
        FaceTarget();

        // Create the 'Projectile' and add to Scene
        Quaternion rotation = Quaternion.Euler(Vector3.zero);
        if (isFacingRight)
        {
            rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
        }

        Rigidbody projectileInstance = Instantiate(projectile, projectileSpawnPoint.position, rotation);
    }

    // Trigger attack when player enters collider range
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (!player.isDead)
            {

                // fire the projectile if ready and player is alive
                if (Time.time > timeSinceLastFire + attackRate)
                {
                    Attack();
                    timeSinceLastFire = Time.time;
                }
            }
        }
    }

    // Walk the other direction if we hit a wall
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            Flip();
        }
    }
}
