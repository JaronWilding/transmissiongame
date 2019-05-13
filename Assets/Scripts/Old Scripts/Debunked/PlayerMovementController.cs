using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 4.0f;
    [SerializeField] private float rayCastDistance = 1.05f;

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private Rigidbody rb;

    




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Jump();
    }
    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(hAxis, 0.0f, vAxis) * speed * Time.fixedDeltaTime;

        Vector3 newPosition = rb.position + rb.transform.TransformDirection(movement);

        rb.MovePosition(newPosition);
    }



    private void Jump()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            if(IsGrounded())
                rb.AddForce(0.0f, jumpForce, 0.0f, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, rayCastDistance);
    }

}
