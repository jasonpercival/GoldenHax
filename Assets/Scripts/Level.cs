﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform spawnLocation;
    public AudioClip backgroundMusic;
    public AudioClip stageClearMusic;

    void Start()
    {
        GameManager.Instance.SpawnPlayer(spawnLocation);
        SoundManager.Instance.PlayMusic(backgroundMusic);
    }
}
