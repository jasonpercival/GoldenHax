﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float lifeTime;

    void Start()
    {
        // Check if 'lifeTime' variable was set in the inspector
        if (lifeTime <= 0)
        {
            // Assign a default value if one was not set
            lifeTime = 2.0f;
            Debug.LogWarning("ProjectileForce not set. Defaulting to " + lifeTime);
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player)
            {
                player.TakeDamage();
            }
            // play projectile hitting animation then destroy projectile
            Destroy(gameObject);
        }
    }

}