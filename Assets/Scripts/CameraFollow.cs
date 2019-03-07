using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Transform target;               // Target the camera will follow
    public Transform camBoundMin;   // Lowest and leftest value camera can move
    public Transform camBoundMax;   // Highest and rightest value camera can move 

    float xMin, xMax, yMin, yMax;

    void Start()
    {
   
        // Find 'Player' in scene
        GameObject go = GameObject.FindWithTag("Player");
        if (!go)
        {
            Debug.Log("Player not found.");
            return;
        }

        target = go.GetComponent<Transform>();

        // Uses GameObjecs to set the min and max values for the camera movement
        xMin = camBoundMin.position.x;
        yMin = camBoundMin.position.y;

        xMax = camBoundMax.position.x;
        yMax = camBoundMax.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = new Vector3(
                Mathf.Clamp(target.position.x, xMin, xMax),
                Mathf.Clamp(target.position.y, yMin, yMax),
                transform.position.z);
        }
    }
}
