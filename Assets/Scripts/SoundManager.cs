using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    static SoundManager _instance = null;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<SoundManager>();
                go.name = "_SoundManager";
                DontDestroyOnLoad(_instance);
                GameManager.Instance.LoadPlayerPrefs();
            }

            return _instance;
        }
    }
    
    void Awake()
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
            else
            {
                Debug.LogError("Audio clip is missing.");
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
