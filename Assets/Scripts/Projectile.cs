using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float lifeTime;

    private Animator anim;

    void Start()
    {
        // Check if 'lifeTime' variable was set in the inspector
        if (lifeTime <= 0)
        {
            // Assign a default value if one was not set
            lifeTime = 2.0f;
            Debug.LogWarning("ProjectileForce not set. Defaulting to " + lifeTime);
        }

        anim = GetComponent<Animator>();
        if (!anim)
        {
            Debug.LogError("Animator missing on " + name);
        }

        Destroy(gameObject, lifeTime);
    }

    public void Hit()
    {
        anim.SetTrigger("Hit");
        
        Destroy(gameObject, 1.0f);
    }


}
