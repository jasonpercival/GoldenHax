using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    public AudioClip bossMusic;

    // triggers boss music when the player enters the end zone of the level
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SoundManager.Instance)
            {
                SoundManager.Instance.PlayMusic(bossMusic);
            }
        }
    }
}
