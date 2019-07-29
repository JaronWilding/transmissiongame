using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastForPlayer : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;

    public WhenSeenTargetPlayer whenSeenTargetPlayer;
    public RayCastForPlayer raycastForPlayer;
    public GameObject TriggerForRaycast;
    public RandomPatrolWaypoints RandomPatrolWaypoints;
    private float sightDist = 10;
    private SphereCollider col;
    public GameObject Player;


    //  public AudioClip zombGrumble;
    //  private AudioSource audioSrc;

    

    

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        //  audioSrc = GetComponent<AudioSource>();
 //       Player = GameObject.FindGameObjectWithTag("GameController");
    }


    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * sightDist, Color.green);
        Debug.DrawRay(transform.position + Vector3.up, (transform.forward + transform.right).normalized * sightDist, Color.red);
        Debug.DrawRay(transform.position + Vector3.up, (transform.forward - transform.right).normalized * sightDist, Color.blue);
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject == Player)
        {

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                

                if (Physics.Raycast(transform.position, direction.normalized * 4, out hit, col.radius))
                {
                    if (hit.collider.gameObject == Player)
                    {
                        Debug.Log("hit");
                        //  audioSrc.PlayOneShot(zombGrumble,1F);
                          TriggerForRaycast.SetActive(false);
                        whenSeenTargetPlayer.enabled = true;                       
                        RandomPatrolWaypoints.enabled = false;
                        raycastForPlayer.enabled = false;

                    }
                }

            }
        }
    }

}