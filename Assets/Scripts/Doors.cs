using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{

    public float moveSpeed = 6.0f;
    public Transform player;
    public float range = 10f;
    Vector3 startPosition, driftPosition;
    Quaternion startRotation, driftRotation;

    public float driftSeconds = 3;
    private float driftTimer = 0;
    private bool isDrifting = false;


	// Use this for initialization
	void Start ()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

	}

    private void StartDrift()

    {
        isDrifting = true;
        driftTimer = 0;

        driftPosition = transform.position;
        driftRotation = transform.rotation;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb !=null)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
	
    private void StopDrift()
    {

        isDrifting = false;
        
        driftPosition = transform.position;
        driftRotation = transform.rotation;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.None;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(player.position, this.transform.position) < range)//range
            {
                Vector3 direction = player.position - this.transform.position;//How it knows to attack

                direction.y = 0;
           
                

                if (direction.magnitude > 10)//Stops moving(but is set to destroy in my player script)
                {
                    this.transform.Translate(0, 0, 0.0f);
                    transform.position += transform.forward * moveSpeed * Time.deltaTime; //Movespeed to attack player
                }
                else //Stops moving(but is set to destroy in my player script)
                {
                    this.transform.Translate(0, 0, 0.0f);
                    transform.position -= transform.forward * moveSpeed * Time.deltaTime; //Stops the move because its -
                }

            }
        StartDrift();

        if (isDrifting)
        {
            driftTimer += Time.deltaTime;
            if (driftTimer > driftSeconds)
            {
                StopDrift();
            }
            else
            {
                float ratio = driftTimer / driftSeconds;
                transform.position = Vector3.Lerp(driftPosition, startPosition, ratio);
               transform.rotation = Quaternion.Slerp(driftRotation, startRotation, ratio);
            }
        }

	}
}
