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
        if (!GameManager.Instance)
        {
            Debug.LogError("Unable to get reference to game manager instance.");
        }

        // get the current settings
        GameManager.Instance.LoadPlayerPrefs();
        sfxVolume.value = SoundManager.Instance.sfxSource.volume;
        musicVolume.value = SoundManager.Instance.musicSource.volume;

        // add listeners to capture volume slider changes and button clicks
        musicVolume.onValueChanged.AddListener(MusicVolumeChanged);
        sfxVolume.onValueChanged.AddListener(SoundVolumeChanged);
        btnCancel.onClick.AddListener(Cancel);
        btnSave.onClick.AddListener(Save);
    }

    internal void PlayTestSound()
    {
        SoundManager.Instance.PlaySound(volumeTestClip);
    }

    private void MusicVolumeChanged(float volume)
    {
        SoundManager.Instance.SetMusicVolume(volume);
    }

    private void SoundVolumeChanged(float volume)
    {
        SoundManager.Instance.SetSoundVolume(volume);
    }

    private void Save()
    {
        // update player prefs with new settings
        GameManager.Instance.SavePlayerPrefs();
        Destroy(gameObject);
    }

    private void Cancel()
    {
        // restore original settings
        GameManager.Instance.LoadPlayerPrefs();
        Destroy(gameObject);
    }
}
