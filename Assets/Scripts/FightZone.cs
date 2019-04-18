using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightZone : MonoBehaviour
{

    public int enemyCounter;
    public GameObject Canvas;

    public GameObject[] enemiesToActivate;

    private GameUI HUD;

    void Start()
    {
        HUD = Canvas.GetComponent<GameUI>();
        if (!HUD)
        {
            Debug.LogError("Unable to get reference to Canvas_HUD");
        }

        if (enemyCounter == 0)
        {
            Debug.LogError("Enemy counter is not set on " + name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered fight zone.");
            GetComponent<Collider>().enabled = false;

            // activate enemies for the fight
            foreach (var enemy in enemiesToActivate)
            {
                enemy.SetActive(true); 
            }
            
            GameManager.Instance.LockBattle();
            GameManager.Instance.OnEnemyKilled += Instance_OnEnemyKilled;

        }
    }

    private void Instance_OnEnemyKilled()
    {
        if (GameManager.Instance.killCount >= enemyCounter)
        {
            GameManager.Instance.OnEnemyKilled -= Instance_OnEnemyKilled;
            GameManager.Instance.UnlockBattle();

            gameObject.SetActive(false);
            HUD.ShowGoIndicator();
        }
    }




}
