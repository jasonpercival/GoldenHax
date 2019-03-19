using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script cleans up particle systems that are no longer active automatically
public class DestroyParticle : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // destroy the particle system if it is no longer playing
        if (ps.isPlaying) return;
        Destroy(gameObject);
    }
}
