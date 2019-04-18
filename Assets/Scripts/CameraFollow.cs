using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Transform target;               // Target the camera will follow
    public Transform camBoundMin;   // Lowest and leftest value camera can move
    public Transform camBoundMax;   // Highest and rightest value camera can move 

    float xMin, xMax, yMin, yMax;

    public bool isTracking;

    public float smoothTime = 0.4f;
    private Vector3 velocity = Vector3.zero;


    void Start()
    {

        // Find 'Player' in scene
        GameObject player = GameManager.Instance.player1;
        if (!player)
        {
            Debug.Log("Player not found.");
            return;
        }

        target = player.GetComponent<Transform>();
        isTracking = true;

        // Uses GameObjecs to set the min and max values for the camera movement
        xMin = camBoundMin.position.x;
        yMin = camBoundMin.position.y;

        xMax = camBoundMax.position.x;
        yMax = camBoundMax.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (target && isTracking)
        {

            Vector3 targetPos = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }
}
