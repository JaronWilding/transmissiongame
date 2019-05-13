using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grates : MonoBehaviour
{
    private Collider wallCollision;
    private Collider myCollider;
    private Collider player;
    private Rigidbody rb;
    private bool timerOn = false;
    private float timer = 1.0f;

    void Start()
    {
        myCollider = GetComponent<Collider>();
        wallCollision = GameObject.FindWithTag("Walls").GetComponent<Collider>();
        player = GameObject.FindWithTag("GameController").GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(myCollider, wallCollision);
    }

    void FixedUpdate()
    {
        if(timerOn)
        {
            timer -= Time.deltaTime;
        }
        if(timer < 0f)
        {
            Physics.IgnoreCollision(myCollider, player, false); 
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "GameController" && Input.GetKeyDown(KeyCode.F))
        {
            Physics.IgnoreCollision(myCollider, player); 
            Physics.IgnoreCollision(myCollider, wallCollision, false);
            rb.useGravity = true;
            rb.AddRelativeForce(Vector3.forward * 20.0f);
            timerOn = true;
        }
    }
}
