using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileLifeTime;
    public bool isTurret;
    public ParticleSystem deathExplosion;

    Rigidbody rb;
    Animator anim;

    private Animator anim;

    void Start()
    {
        // Check if 'projectileLifeTime' variable was set in the inspector
        if (projectileLifeTime <= 0)
        {
            projectileLifeTime = 2.0f;
            Debug.LogWarning("ProjectileForce not set. Defaulting to " + projectileLifeTime);
        }

<<<<<<< HEAD
=======
        // Check if 'projectileSpeed' variable was set in the inspector
        if (projectileSpeed <= 0)
        {
            projectileSpeed = 5.0f;
            Debug.LogWarning("ProjectileSpeed not set. Defaulting to " + projectileSpeed);
        }

        Destroy(gameObject, projectileLifeTime);

>>>>>>> 8f04add5ebb23dbd862e1ca854f98012863272b7
        anim = GetComponent<Animator>();
        if (!anim)
        {
            Debug.LogError("Animator missing on " + name);
        }

<<<<<<< HEAD
        Destroy(gameObject, lifeTime);
    }

    public void Hit()
    {
        anim.SetTrigger("Hit");
        
        Destroy(gameObject, 1.0f);
    }

=======
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogError("Rigidbody not found on " + name);
        }

        if (isTurret)
        {
            // shoot projectile into the air with gravity
            rb.useGravity = true;
            if (transform.localRotation.y == 0)
            {
                rb.AddForce(new Vector3(-0.5f, 1.5f) * projectileSpeed, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(new Vector3(0.5f, 1.5f) * projectileSpeed, ForceMode.Impulse);
            }
        }
        else
        {
            // shoot projectile straight 
            if (transform.localRotation.y == 0)
            {
                rb.AddForce(Vector3.left * projectileSpeed, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(Vector3.right * projectileSpeed, ForceMode.Impulse);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        // projectile hit object
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
            anim.SetTrigger("Hit");
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage();
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);

        if (deathExplosion)
        {
            ParticleSystem temp = Instantiate(deathExplosion, transform.position, transform.rotation);

            // Length of partical system life
            Destroy(temp, deathExplosion.main.duration);
        }
    }

>>>>>>> 8f04add5ebb23dbd862e1ca854f98012863272b7

}
