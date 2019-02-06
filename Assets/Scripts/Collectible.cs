using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int healthBonus = 0;
    public int potionBonus = 0;
    public AudioClip collectItemClip;    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collisions only with player
        if (collision.gameObject.tag == "Player")
        {
            // Get the players character script component
            Character character = collision.GetComponent<Character>();
            if (!character)
            {
                Debug.LogWarning("Unable to get player character script component on " + name);
            }
            else
            {

                AudioSource audioSource = collision.GetComponent<AudioSource>();
                if (audioSource && collectItemClip)
                {
                    audioSource.PlayOneShot(collectItemClip);
                }

                character.PickupItem();

                // If this is a food collectible increment the player health bar
                if (healthBonus > 0)
                {
                    character.currentHealth += healthBonus;
                    if (character.currentHealth > character.maxHealth)
                    {
                        character.currentHealth = character.maxHealth;
                    }
                }

                // If this is a potion collectible increase the player potions
                if (potionBonus > 0)
                {
                    character.currentPotions += potionBonus;
                    if (character.currentPotions > character.maxPotions)
                    {
                        character.currentPotions = character.maxPotions;
                    }
                }
     
            }

            // Remove the collectible from the scene
            Destroy(gameObject);
        }
    }
}
