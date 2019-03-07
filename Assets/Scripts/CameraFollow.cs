using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothX;
    public float smoothY;

    public GameObject target;
    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

    public float boundsX;

    private Vector2 velocity;

    void Start()
    {
        if (!target)
        {
            // get transform of player character
            target = GameManager.instance.player1;
        }
    }

    private bool CheckBounds()
    {

        float distance = Mathf.Abs(transform.position.x - target.transform.position.x);

        Debug.Log("CameraX: " + transform.position.x + ", PlayerX: " + target.transform.position.x + ", Diff: " + distance);

        return distance > boundsX;

    }

    private void LateUpdate()
    {

        if (target)
        {

            float posX = transform.position.x;
            float posY = transform.position.y;

            //if (CheckBounds())
            //{
            posX = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothX);
            posY = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothY);
            //}

            transform.position = new Vector3(Mathf.Clamp(posX, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(posY, minCameraPos.y, maxCameraPos.y), transform.position.z);

        }

    }
}
