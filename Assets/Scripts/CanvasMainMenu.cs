using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMainMenu : MonoBehaviour
{
    public Button btnStart;
    public Button btnOptions;
    public GameObject optionsMenu;
    public AudioClip titleMusic;

    void Start()
    {

        btnStart.onClick.AddListener(GameManager.Instance.StartGame);
        btnOptions.onClick.AddListener(ShowOptions);
        GameManager.Instance.SetGameState(GameState.MAIN_MENU);
        SoundManager.Instance.PlayMusic(titleMusic);
    }

    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            GameManager.Instance.StartGame();
        }    
    }

    void ShowOptions()
    {
        if (optionsMenu)
        {
            Instantiate(optionsMenu);
        }
    }
}
