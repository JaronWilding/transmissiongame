using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastforPlayer : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;

    public WhenSeenTargetPlayer whenSeenTargetPlayer;
    public RayCastforPlayer rayCast;
    public GameObject TriggerforRaycast;
    public RandomPatrolWaypoints RandomPatrolWaypoints;

    public AudioClip zombGrumble;
    private AudioSource audioSrc;

    private SphereCollider col;

    public GameObject Player;

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        audioSrc = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {

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
                        audioSrc.PlayOneShot(zombGrumble,1F);
                        whenSeenTargetPlayer.enabled = true;
                        TriggerforRaycast.SetActive(false);
                        rayCast.enabled = false;
                        RandomPatrolWaypoints.enabled = false;

                    }
                }

            }
        }
    }

}