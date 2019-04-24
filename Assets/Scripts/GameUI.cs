using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject[] player1_HealthBars;
    public Text player1Lives;
    public Text gameOver;
    public Text stageClear;
    public Animator goAnimator;
    public AudioClip goIndicator;
    public FightZone endZone;

    private bool zoneActive;

    Player player1;

    private void Start()
    {
        player1 = GameManager.Instance.player1.GetComponent<Player>();
        if (!player1)
        {
            Debug.LogError("Unable to get reference to player 1 in " + name);
        }

        if (!goAnimator)
        {
            Debug.LogError("Unable to get reference to animator component.");
        }
        zoneActive = true;
    }

    public void ShowGoIndicator()
    {
        goAnimator.SetTrigger("Indicator");
        StartCoroutine(GoIndicator());
    }

    // Make the player sprite flash temporarily with a given color
    IEnumerator GoIndicator()
    {
        for (int i = 0; i < 3; i++)
        {
            SoundManager.Instance.PlaySound(goIndicator);
            yield return new WaitForSeconds(goIndicator.length);
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

        // check for stage clear
        if (zoneActive)
        {
            bool enemyAlive = false;
            foreach (var enemy in endZone.enemiesToActivate)
            {
                if (enemy)
                {
                    enemyAlive = true;
                    break;
                }
            }

            if (!enemyAlive)
            {
                stageClear.enabled = true;
                zoneActive = false;

                // restart
                var level = GameObject.Find("Level1").GetComponent<Level>();
                SoundManager.Instance.PlayMusic(level.stageClearMusic, loop: false);
                GameManager.Instance.StageClear();
            }
        }
    }
}
