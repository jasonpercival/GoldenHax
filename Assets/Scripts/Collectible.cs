using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int healthBonus = 0;
    public int potionBonus = 0;
    public AudioClip collectItemClip;
    public ParticleSystem collectParticle;
    
    private void OnTriggerEnter(Collider other)
    {
        // Check for collisions only with player
        if (other.tag == "Player")
        {
            // Get the players character script component
            Player player = other.GetComponent<Player>();
            if (!player)
            {
                Debug.LogWarning("Unable to get player character script component on " + name);
            }
            else
            {
                // Let player script know about the collectible
                player.CollectItem(healthBonus, potionBonus);

                // collect item particle visual effect
                if (collectParticle)
                {
                    // note: particle system will destroy itself after it's play duration
                    Instantiate(collectParticle, other.transform.position, Quaternion.identity);
                }
            }

            // Collect item sound effect
            if (collectItemClip)
            {
                SoundManager.Instance.PlaySound(collectItemClip);
            }

            // Remove the collectible from the scene
            Destroy(gameObject);
        }
    }

}
