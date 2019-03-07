using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject[] player1_HealthBars;
    public Text player1Lives;
    public Text gameOver;

    Player player1;

    private void Start()
    {
        player1 = GameManager.instance.player1.GetComponent<Player>();
        if (!player1)
        {
            Debug.LogError("Unable to get reference to player 1 in " + name);
        }
    }

    void Update()
    {
        // update player 1 lives left
        player1Lives.text = player1.lives.ToString();
        if (player1.lives < 1)
        {
            gameOver.enabled = true;
        }

        // 2 health = 1 health bar
        int numberOfBars = (player1.health + 1) / 2;

        // update visibility of the health bars
        for (int i = 0; i < player1_HealthBars.Length; i++)
        {
            player1_HealthBars[i].SetActive(i < numberOfBars);
        }
    }
}
