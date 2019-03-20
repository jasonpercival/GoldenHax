using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { NullState, MainMenu, Game };

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    private GameState _gs = GameState.NullState;

    public GameState gs
    {
        get { return _gs; }
        set
        {
            _gs = value;
            Debug.Log("Current State: " + _gs);
        }
    }

    public GameObject playerPrefab;
    public GameObject player1 { get; set; }

    // player preferences
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SOUND_VOLUME = "SoundVolume";

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

        gs = GameState.MainMenu;
    }

    public void LoadPlayerPrefs()
    {
        // music volume
        if (PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            SoundManager.instance.musicSource.volume = PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
        else
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, SoundManager.instance.musicSource.volume);
            PlayerPrefs.Save();
        }

        // sound volume
        if (PlayerPrefs.HasKey(SOUND_VOLUME))
        {
            SoundManager.instance.sfxSource.volume = PlayerPrefs.GetFloat(SOUND_VOLUME);
        }
        else
        {
            PlayerPrefs.SetFloat(SOUND_VOLUME, SoundManager.instance.sfxSource.volume);
            PlayerPrefs.Save();
        }
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME, SoundManager.instance.musicSource.volume);
        PlayerPrefs.SetFloat(SOUND_VOLUME, SoundManager.instance.sfxSource.volume);
        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        LoadLevel("Level1");
    }

    public void LoadMainMenu()
    {
        LoadLevel("MainMenu");
    }

    public void LoadLevel(string levelName)
    {
        SoundManager.instance.sfxSource.mute = true;

        switch (levelName)
        {
            case "MainMenu":
                gs = GameState.MainMenu;
                SoundManager.instance.PlayMusic(SoundManager.instance.titleScreen);
                break;
            case "Level1":
                gs = GameState.Game;
                SoundManager.instance.PlayMusic(SoundManager.instance.stage1);
                break;
            default:
                gs = GameState.NullState;
                SoundManager.instance.PlayMusic(null);
                break;
        }

        SceneManager.LoadScene(levelName);
    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        if (playerPrefab && spawnLocation)
        {
            player1 = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        }
    }

    public void GameOver()
    {
        SoundManager.instance.PlayMusic(SoundManager.instance.gameOver, loop: false);
        Invoke("LoadMainMenu", 5);
    }
}
