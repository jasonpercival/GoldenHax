using UnityEngine;

public class Parallaxing : MonoBehaviour
{

    public float smoothingSpeed = 1f;  // scrolling smooth speed
    public Transform[] backgrounds;     // references to all the backgrounds that will be scrolled

    private Transform mainCamera;       // reference to the main cameras transform  
    private Vector3 oldCameraPosition;  // previous position of the camera in the last update
    private float[] scrollSpeeds;       // holds the amount to scroll each background

    void Awake()
    {
        // get reference to camera transform
        mainCamera = Camera.main.transform; 
    }

   void Start()
    {
        // update previous camera position with current camera position
        oldCameraPosition = mainCamera.position;  

        // initialize the scrolling speeds based on the current Z position of each background
        scrollSpeeds = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            scrollSpeeds[i] = backgrounds[i].position.z * -1; // background moves opposite direction to camera
        }
    }

    void Update()
    {
        // adjust each backgrounds X position based by its calculated scrolling speed
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (oldCameraPosition.x - mainCamera.position.x) * scrollSpeeds[i];   // calculate amount of movement
            float targetPositionX = backgrounds[i].position.x + parallax;                       // calculate the target X position of the background

            // lerp from current position to target position for smooth scrolling
            Vector3 newPosition = new Vector3(targetPositionX, backgrounds[i].position.y, backgrounds[i].position.z); 
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, newPosition, smoothingSpeed * Time.deltaTime);
        }

        // update previous camera position with current camera position
        oldCameraPosition = mainCamera.position;
    }
}