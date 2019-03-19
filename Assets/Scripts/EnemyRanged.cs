using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{
    public float fireRate;
    public Rigidbody projectile;            // What projectile to spawn 
    public Transform projectileSpawnPoint;  // Where to spawn projectile
    public float projectileForce;           // How fast is projectile

    float timeSinceLastFire;

    Rigidbody rb;

    void Start()
    {
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

        // Check if 'projectileForce' variable was set in the inspector
        if (projectileForce <= 0)
        {
            // Assign a default value if one was not set
            projectileForce = 5.0f;
            Debug.LogWarning("ProjectileForce not set. Defaulting to " + projectileForce);
        }
        // Check if 'fireRate' variable was set in the inspector
        if (fireRate <= 0)
        {
            // Assign a default value if one was not set
            fireRate = 2.0f;
            Debug.LogWarning("fireRate not set. Defaulting to " + fireRate);
        }

        rb = GetComponent<Rigidbody>();
    }

<<<<<<< HEAD
    public override void Attack()
=======
    // Update is called once per frame
    void Update()
>>>>>>> e9d96f33be5e910b37aa909d5a6c9be772947915
    {

    }

    public void Fire()
    {
        // Check if 'projectileSpawnPoint' and 'projectile' exist
        if (projectileSpawnPoint && projectile)
        {
            // Create the 'Projectile' and add to Scene
            Rigidbody temp = Instantiate(projectile, projectileSpawnPoint.position,
                projectileSpawnPoint.rotation);

<<<<<<< HEAD
        Rigidbody projectileInstance = Instantiate(projectile, projectileSpawnPoint.position, rotation);
=======
            // Stop 'Enemy' from hitting 'Projectile'
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), temp.GetComponent<CapsuleCollider>(), true);

            // Check what direction 'Character' is facing before firing
            if (isFacingRight)
            {
                temp.transform.Rotate(0, 180, 0);
                temp.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode.Impulse);
            }
            else
                temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode.Impulse);
        }
>>>>>>> e9d96f33be5e910b37aa909d5a6c9be772947915
    }


    // Trigger when player enters attack range
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // face the player first
            Vector3 difference = other.gameObject.transform.position - transform.position;
            Debug.Log(difference.x + " isFacingRight: " + isFacingRight);

<<<<<<< HEAD
                // fire the projectile if ready and player is alive
                if (Time.time > timeSinceLastFire + attackRate)
                {
                    Attack();
                    timeSinceLastFire = Time.time;
                }
=======
            if (difference.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (difference.x < 0 && isFacingRight)
            {
                Flip();
>>>>>>> e9d96f33be5e910b37aa909d5a6c9be772947915
            }

            if (Time.time > timeSinceLastFire + fireRate)
            {
                Fire();
                timeSinceLastFire = Time.time;
            }
        }
    }
}
