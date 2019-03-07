using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { NullState, MainMenu, Game };

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    public GameObject playerPrefab;

    [Header("Audio")]
    public AudioClip titleScreen;
    public AudioClip stage1;
    public AudioClip gameOver;

    private GameObject _player1;
    private GameState _gs = GameState.NullState;
    private AudioSource audioSrc;

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

    public void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        if (!audioSrc)
        {
            Debug.LogError("AudioSource component not found on " + name);
        }
    }

    public void StartGame()
    {
        LoadLevel("Level1");
    }

    public void LoadMainMenu()
    {
        LoadLevel("MainMenu");
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (audioSrc.isPlaying)
            audioSrc.Stop();

        if (audioClip)
        {
            audioSrc.clip = audioClip;
            audioSrc.Play();
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (audioClip)
        {
            audioSrc.PlayOneShot(audioClip);
        }
    }

    public void LoadLevel(string levelName)
    {
        switch (levelName)
        {
            case "MainMenu":
                gs = GameState.MainMenu;
                PlayMusic(titleScreen);
                break;
            case "Level1":
                gs = GameState.Game;
                PlayMusic(stage1);
                break;
            default:
                gs = GameState.NullState;
                PlayMusic(null);
                break;
        }

        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SpawnPlayer(Transform spawnLocation)
    {
        if (playerPrefab && spawnLocation)
        {
            player1 = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
        }
    }

    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public GameState gs
    {
        get { return _gs; }
        set
        {
            _gs = value;
            Debug.Log("Current State: " + _gs);
        }
    }

    public GameObject player1
    {
        get { return _player1; }
        set { _player1 = value; }
    }

    internal void GameOver()
    {
        PlayMusic(gameOver);
        Invoke("LoadMainMenu", 5);
    }
}
