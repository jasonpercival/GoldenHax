using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio")]
    public AudioClip titleScreen;
    public AudioClip stage1;
    public AudioClip gameOver;

    static SoundManager _instance = null;

    public static SoundManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        // initialize audio sources
        musicSource = gameObject.AddComponent<AudioSource>();
        if (musicSource)
        {
            musicSource.playOnAwake = false;
            musicSource.spatialBlend = 0.0f;
            musicSource.loop = true;
            musicSource.volume = 1.0f;
        }

        sfxSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource)
        {
            sfxSource.playOnAwake = false;
            sfxSource.spatialBlend = 0.0f;
            sfxSource.loop = false;
            sfxSource.volume = 1.0f;
        }

        GameManager.instance.LoadPlayerPrefs();
        PlayMusic(titleScreen);
    }

    public void PlayMusic(AudioClip audioClip, bool loop = true)
    {
        if (musicSource)
        {
            if (musicSource.isPlaying)
                musicSource.Stop();

            if (audioClip)
            {
                musicSource.clip = audioClip;
                musicSource.loop = loop;
                musicSource.Play();
            }
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (sfxSource && audioClip)
        {
            sfxSource.PlayOneShot(audioClip);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        sfxSource.volume = volume;
    }



}
