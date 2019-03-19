using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
    public float speed;
    public Image bg;

    void Start()
    {
        // check if speed is set in the inspector
        if (speed <= 0)
        {
            speed = 60.0f;
        }

        bg = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {

        float y = Mathf.Repeat(Time.time * speed, 1);
        bg.material.SetTextureOffset("_MainTex", new Vector2(0,  y));
    }
}
