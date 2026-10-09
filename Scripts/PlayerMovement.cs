using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 7;
    public float horizontalSpeed = 3;
    public float rightLimit = 5.5f;
    public float leftLimit = -5.5f;
    public float upLimit = 5.5f;
    public float jumpSpeed = 10f;
    public float gravity = 19f;

    [Header("Amy Animation")]
    [SerializeField] private Animator amyAnimator;

    private float verticalVelocity = 0f;
    private bool isGrounded = true;
    private float groundY;

    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    void Start()
    {
        groundY = transform.position.y;
    }

    void Update()
    {
        // Forward movement
        transform.Translate(
            Vector3.forward * Time.deltaTime * playerSpeed,
            Space.World
        );

        // Move left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > leftLimit)
            {
                transform.Translate(
                    Vector3.left * Time.deltaTime * horizontalSpeed
                );
            }
        }

        // Move right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x < rightLimit)
            {
                transform.Translate(
                    Vector3.left * Time.deltaTime *
                    horizontalSpeed * -1
                );
            }
        }

        // Jump
        if ((Input.GetKeyDown(KeyCode.Space) ||
             Input.GetKeyDown(KeyCode.UpArrow)) &&
             isGrounded)
        {
            verticalVelocity = jumpSpeed;
            isGrounded = false;

            // Play Jump animation
            if (amyAnimator != null)
            {
                amyAnimator.Play("Jump");
            }
        }

        // Apply gravity while jumping
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;

            Vector3 position = transform.position;
            position.y += verticalVelocity * Time.deltaTime;
            transform.position = position;

            // Landing
            if (transform.position.y <= groundY)
            {
                position = transform.position;
                position.y = groundY;
                transform.position = position;

                verticalVelocity = 0f;
                isGrounded = true;

                // Return to Running animation
                if (amyAnimator != null)
                {
                    amyAnimator.Play("Running");
                }
            }
        }
    }
}