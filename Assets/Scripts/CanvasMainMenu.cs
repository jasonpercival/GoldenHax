using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMainMenu : MonoBehaviour
{
    public Button btnStart;
    public Button btnOptions;
    public GameObject optionsMenu;

    void Start()
    {
        if (GameManager.instance)
        {
            btnStart.onClick.AddListener(GameManager.instance.StartGame);
            btnOptions.onClick.AddListener(ShowOptions);
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
