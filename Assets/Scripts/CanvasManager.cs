using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Button btnStart;

    void Start()
    {
        if (GameManager.instance && btnStart)
            btnStart.onClick.AddListener(GameManager.instance.StartGame);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
