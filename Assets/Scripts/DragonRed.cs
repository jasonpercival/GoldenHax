using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonRed : Enemy
{
    public Rigidbody projectile;            // What projectile to spawn 
    public Transform projectileSpawnPoint;  // Where to spawn projectile
    public float projectileForce;           // How fast is projectile

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            StartCoroutine(CastFireBall());
        }
    }

    public void Fire()
    {
        // Check if 'projectileSpawnPoint' and 'projectile' exist
        if (projectileSpawnPoint && projectile)
        {
            // Create the 'Projectile' and add to Scene
            Rigidbody temp = Instantiate(projectile, projectileSpawnPoint.position,
                projectileSpawnPoint.rotation);

            // Stop 'Enemy' from hitting 'Projectile'
            Physics.IgnoreCollision(GetComponent<CapsuleCollider>(),temp.GetComponent<CapsuleCollider>(), true);

            // Check what direction 'Character' is facing before firing
            if (isFacingRight)
                temp.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode.Impulse);
            else
                temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode.Impulse);
        }
    }

    IEnumerator CastFireBall()
    {
        isAttacking = true;
        yield return new WaitForSeconds(2.0f);
        Fire();
        isAttacking = false;
    }
}
