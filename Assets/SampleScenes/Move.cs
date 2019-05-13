using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public SphereCollider topSphere;
    public SphereCollider botSphere;

    public Rigidbody rb;

    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 5.0f);


        /*


        velocity += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 vel = velocity * 5.0f;
        //velocity = new Vector3(move.x, -currentGravity + jumpHeight, move.z) * movementSpeed;
        //velocity = transform.TransformDirection(velocity);
        vel = transform.TransformDirection(vel);
        transform.position += vel * Time.deltaTime;
        velocity = Vector3.zero;*/
    }
}
