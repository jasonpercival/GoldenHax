using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    CanvasGroup cg;
    public Button btnPause;

    void Start()
    {
        // start with the pause menu hidden
        cg = GetComponent<CanvasGroup>();
        if (!cg)
        {
            cg.gameObject.AddComponent<CanvasGroup>();
        }
        cg.alpha = 0.0f;

        // link pause button to pause method
        if (btnPause)
            btnPause.onClick.AddListener(PauseGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    // show/hide pause menu
    public void PauseGame()
    {
        if (cg.alpha == 0.0f)
        {
            cg.alpha = 1.0f;
            Time.timeScale = 0.0f;
        }
        else
        {
            cg.alpha = 0.0f;
            Time.timeScale = 1.0f;
        }
    }
}