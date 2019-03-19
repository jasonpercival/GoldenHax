using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
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
}
