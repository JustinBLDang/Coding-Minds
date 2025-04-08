using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] float grav = 1f;
    [SerializeField] float default_speed = 1f;
    [SerializeField] float camera_sensitivity = 300f;
    [SerializeField] float groundCheckDistance = .3f;
    [SerializeField] float jumpPower = 5.0f;
    float maxVerticalVelocity;
    float movement_speed;
    float vertical_Rotation;
    float horizontal_Rotation;

    float mouseX;
    float mouseY;

    float X_Input;
    float Y_Input;
    LayerMask groundMask;
    bool isGrounded = false;
    float playerheight;
    Vector2 movementDirection;
    RaycastHit slopeHit;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    Camera camera;
    #endregion Variables

    void Start(){
        camera = GetComponentInChildren<Camera>();
        vertical_Rotation = transform.rotation.y;
        horizontal_Rotation = transform.rotation.x;
        rb = gameObject.GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        movement_speed = default_speed;
        playerheight = capsuleCollider.height;

        // ~ operator: bit-wise complement 
        groundMask = ~(1 << LayerMask.NameToLayer("Player"));

        // Hide Mouse and prevent it from going outside the window
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();

        // Camera orientation input
        mouseX = Input.GetAxis("Mouse X") * camera_sensitivity * Time.deltaTime; // Explain Time.deltaTime in a bit
        mouseY = Input.GetAxis("Mouse Y") * camera_sensitivity * Time.deltaTime;

        // Player movement input
        Y_Input = Input.GetAxisRaw("Vertical");
        X_Input = Input.GetAxisRaw("Horizontal");

        // Camera control with Mouse:
        Camera();

        // Jumping...
        if (Input.GetKeyDown("space"))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        // Movement control
        Move();

        // Control Fall speed
        if(!isGrounded){
            rb.AddForce(Physics.gravity * grav, ForceMode.Acceleration);
        }
        else{
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }

    private void Move()
    {
        movementDirection.x = X_Input;
        movementDirection.y = Y_Input;
        if (movementDirection.magnitude != 0)
        {
            Vector3 input;
            Physics.Raycast(transform.position, Vector3.down, out slopeHit, Mathf.Infinity, groundMask);
            if (isGrounded)
            {
                input = Vector3.ProjectOnPlane(transform.forward * movementDirection.y + transform.right * movementDirection.x, slopeHit.normal);
            }
            else
            {
                input = transform.forward * movementDirection.y + transform.right * movementDirection.x;
            }

            // Q: Time.deltaTime? 
            rb.MovePosition(transform.position + (input.normalized * movement_speed * Time.deltaTime));

            // Sprinting
            if (Input.GetKey("left shift"))
            {
                movement_speed = default_speed * 1.5f;
            }
            else
            {
                movement_speed = default_speed;
            }
        }
        else {
            Stop();
        }
    }

    void Stop()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    private void Camera()
    {
        horizontal_Rotation += mouseX;
        vertical_Rotation -= mouseY;
        

        // camera orientation
        vertical_Rotation = Mathf.Clamp(vertical_Rotation, -90f, 90f);
        camera.transform.rotation = Quaternion.Euler(vertical_Rotation, horizontal_Rotation, 0);

        // Orient player so they face the same direction as the camera
        rb.MoveRotation(Quaternion.Euler(0, camera.transform.eulerAngles.y, 0));
    }

    void CheckGrounded(){
        // playerheight / 2 = bottom of capsule since transform is at exact center
        if (Physics.Raycast(transform.position, Vector3.down, (playerheight / 2) + groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
