using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothX;
    public float smoothY;

    public GameObject player;
    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

    public float boundsX;

    private Vector2 velocity;

    void Start()
    {
        if (!player)
        {
            // get transform of player character
            player = GameObject.Find("Player");
        }
    }

    private bool CheckBounds()
    {

        float distance = Mathf.Abs(transform.position.x - player.transform.position.x);

        Debug.Log("CameraX: " + transform.position.x + ", PlayerX: " + player.transform.position.x + ", Diff: " + distance);

        return distance > boundsX;

    }

    private void LateUpdate()
    {

        float posX = transform.position.x;
        float posY = transform.position.y;

        //if (CheckBounds())
        //{
            posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothX);
            posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothY);
        //}

        transform.position = new Vector3(Mathf.Clamp(posX, minCameraPos.x, maxCameraPos.x),
            Mathf.Clamp(posY, minCameraPos.y, maxCameraPos.y), transform.position.z);

    }
}
