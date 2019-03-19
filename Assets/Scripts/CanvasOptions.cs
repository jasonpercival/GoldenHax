using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOptions : MonoBehaviour
{
    public Button btnCancel;
    public Button btnSave;
    public Slider sfxVolume;
    public Slider musicVolume;
    public AudioClip volumeTestClip;

    void Start()
    {
        if (!GameManager.instance)
        {
            Debug.LogError("Unable to get reference to game manager instance.");
        }

        // get the current settings
        GameManager.instance.LoadPlayerPrefs();
        sfxVolume.value = SoundManager.instance.sfxSource.volume;
        musicVolume.value = SoundManager.instance.musicSource.volume;

        // add listeners to capture volume slider changes and button clicks
        musicVolume.onValueChanged.AddListener(MusicVolumeChanged);
        sfxVolume.onValueChanged.AddListener(SoundVolumeChanged);
        btnCancel.onClick.AddListener(Cancel);
        btnSave.onClick.AddListener(Save);
    }

    internal void PlayTestSound()
    {
        SoundManager.instance.PlaySound(volumeTestClip);
    }

    private void MusicVolumeChanged(float volume)
    {
        SoundManager.instance.SetMusicVolume(volume);
    }

    private void SoundVolumeChanged(float volume)
    {
        SoundManager.instance.SetSoundVolume(volume);
    }

    private void Save()
    {
        // update player prefs with new settings
        GameManager.instance.SavePlayerPrefs();
        Destroy(gameObject);
    }

    private void Cancel()
    {
        // restore original settings
        GameManager.instance.LoadPlayerPrefs();
        Destroy(gameObject);
    }
}
