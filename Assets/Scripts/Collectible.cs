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
    
    private void OnDestroy()
    {
        // Collect item sound effect
        if (collectItemClip && GameManager.instance)
        {
            SoundManager.instance.PlaySound(collectItemClip);           
        }
    }

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
                    ParticleSystem temp = Instantiate(collectParticle, other.transform.position, Quaternion.identity);
                    // destory object after length of partical system life
                    Destroy(temp, collectParticle.main.duration);
                }
            }

            // Remove the collectible from the scene
            Destroy(gameObject);
        }
    }

}
