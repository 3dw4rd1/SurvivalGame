using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 1.2f; // Increase this value
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Use CharacterController.isGrounded as primary check
        isGrounded = controller.isGrounded;

        // Perform an additional check using SphereCast
        if (!isGrounded)
        {
            isGrounded = Physics.SphereCast(transform.position + Vector3.up * 0.1f, controller.radius, Vector3.down, out RaycastHit hit, groundDistance, groundMask);
        }

        Debug.Log($"Is Grounded: {isGrounded}, Position: {transform.position}, Controller.isGrounded: {controller.isGrounded}, SphereCast Hit: {(isGrounded && Physics.SphereCast(transform.position + Vector3.up * 0.1f, controller.radius, Vector3.down, out RaycastHit hitInfo, groundDistance, groundMask) ? hitInfo.collider.name : "None")}");

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //right is the red Axis, foward is the blue axis
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        //check if the player is on the ground so he can jump
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump button pressed");
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                Debug.Log("Jump initiated, new Velocity Y: " + velocity.y);
            }
            else
            {
                Debug.Log("Jump failed - not grounded");
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}