using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int healthBonus = 0;
    public int potionBonus = 0;
    public AudioClip collectItemClip;

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
                // Collect item sound effect
                AudioSource audioSource = GameObject.FindWithTag("GameController").GetComponent<AudioSource>();
                if (audioSource && collectItemClip)
                {
                    audioSource.PlayOneShot(collectItemClip);
                }

                // Let player script know about the collectible
                player.CollectItem(healthBonus, potionBonus);
            }

            // Remove the collectible from the scene
            Destroy(gameObject);
        }
    }

}
