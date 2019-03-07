using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform spawnLocation;

    void Start()
    {
        if (GameManager.instance)
        {
            GameManager.instance.SpawnPlayer(spawnLocation);
        }
    }
}
