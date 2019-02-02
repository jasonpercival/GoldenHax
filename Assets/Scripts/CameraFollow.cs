using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 cameraOffset;

    Animator anim;

    void Start()
    {
        if (!player)
        {
            // get tranform of player character
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        // get offset of camera from players position
        cameraOffset = transform.position - player.transform.position;

        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // determine the new camera position
        Vector3 targetPosition = player.transform.position + cameraOffset;

        // freeze the Y position of the camera
        targetPosition.y = transform.position.y;

        // update camera's position to follow the player
        transform.position = targetPosition;

    }
}
