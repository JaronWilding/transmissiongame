using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{

    //User Variables
    [SerializeField] private float walkAcceleration = 5.0f;
    [SerializeField] private float maxWalkSpeed = 20.0f;

    //Variables
    private Rigidbody rb;
    private Vector2 horizontalMovement;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerMovement_addForce();
    }


    private void PlayerMovement_Vel()
    {

    }

    private void PlayerMovement_addForce()
    {
        horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z);

        if(horizontalMovement.magnitude > maxWalkSpeed)
        {
            horizontalMovement.Normalize();
            horizontalMovement *= maxWalkSpeed;
        }

        rb.velocity = new Vector3(horizontalMovement.x, 0, horizontalMovement.y);
        transform.rotation = Quaternion.Euler(0, NewPlayerLook.currentYRotation, 0);
        rb.AddRelativeForce(Input.GetAxis("Horizontal") * walkAcceleration, 0, Input.GetAxis("Vertical") * walkAcceleration);
    }
}
