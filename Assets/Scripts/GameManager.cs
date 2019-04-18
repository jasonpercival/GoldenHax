using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { NULLSTATE, GAME, MAIN_MENU }

public delegate void OnStateChangeHandler();
public delegate void OnEnemyKilledHandler();

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    public GameState gameState { get; private set; }

    public bool IsBattleLocked = false; 
    public int killCount = 0;

    public GameObject playerPrefab;
    public GameObject player1 { get; set; }

    // player preferences
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SOUND_VOLUME = "SoundVolume";

    public event OnStateChangeHandler OnStateChange;
    public event OnEnemyKilledHandler OnEnemyKilled;

    public void SetGameState(GameState state)
    {
        gameState = state;
        if (OnStateChange != null)
            OnStateChange();
    }


    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<GameManager>();
                _instance.playerPrefab = Resources.Load("Warrior", typeof(GameObject)) as GameObject;
                go.name = "_GameManager";
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }

    public void LoadPlayerPrefs()
    {
        // music volume
        if (PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            SoundManager.Instance.musicSource.volume = PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
        else
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, SoundManager.Instance.musicSource.volume);
            PlayerPrefs.Save();
        }

        // sound volume
        if (PlayerPrefs.HasKey(SOUND_VOLUME))
        {
            SoundManager.Instance.sfxSource.volume = PlayerPrefs.GetFloat(SOUND_VOLUME);
        }
        else
        {
            PlayerPrefs.SetFloat(SOUND_VOLUME, SoundManager.Instance.sfxSource.volume);
            PlayerPrefs.Save();
        }
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME, SoundManager.Instance.musicSource.volume);
        PlayerPrefs.SetFloat(SOUND_VOLUME, SoundManager.Instance.sfxSource.volume);
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
        switch (levelName)
        {
            case "MainMenu":
                SetGameState(GameState.MAIN_MENU);
                break;
            case "Level1":
                SetGameState(GameState.GAME);
                break;
            default:
                SetGameState(GameState.NULLSTATE);
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
        Invoke("LoadMainMenu", 5);
    }

    // Locks the camera and player movement within the camera's view during battle
    public void LockBattle()
    {
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.isTracking = false;

        float horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
  
        // lock the player to the current camera position until fight is over
        Player player = player1.GetComponent<Player>();
        player.MinPlayerX = Camera.main.transform.position.x - horizontalExtent;
        player.MaxPlayerX = Camera.main.transform.position.x + horizontalExtent;
    }

    public void UpdateKills()
    {
        killCount++;
        if (OnEnemyKilled != null)
        {
            OnEnemyKilled();
        }
    }

    // Unlocks the player after a battle to move freely again
    public void UnlockBattle()
    {
        Player player = player1.GetComponent<Player>();
        player.MaxPlayerX = 38.0f;
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.isTracking = true;
    }

}


