﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{

    private AudioSource audioSrc;
    public AudioClip bossMusic;

    private void Start()
    {
        audioSrc = GameObject.FindWithTag("GameController").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // switch to the boss zone music
            if (audioSrc.isPlaying)
                audioSrc.Stop();

            audioSrc.clip = bossMusic;
            audioSrc.Play();
        }
    }
}
