using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 1.0f;

    private Animator animator;
    private SpriteRenderer sr;
    private float horizontalMovement = 0.0f;
    private float verticalMovement = 0.0f;
    private bool FacingLeft = false;            // previous direction player is facing

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // flip the player sprite if facing left
        if (horizontalMovement > 0)
        {
            FacingLeft = false;
        }
        else if (horizontalMovement < 0)
        {
            FacingLeft = true;
        }

        sr.flipX = FacingLeft;
    }

    private void FixedUpdate()
    {
        // pass movement values to animator to handle animation transitions
        if (animator)
        {
            animator.SetFloat("horizontalMovement", horizontalMovement);
            animator.SetFloat("verticalMovement", verticalMovement);
        }

        // move the player by the desired speed horizontally and vertically
        transform.Translate(horizontalMovement * horizontalSpeed * Time.fixedDeltaTime, verticalMovement * verticalSpeed * Time.fixedDeltaTime, 0);
    }
}
