using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhenSeenTargetPlayer : MonoBehaviour
{
    NavMeshAgent myAgent;
    public Transform[] points;
    public int destinationPoint = 0;
    public float moveSpeed = 15f;
    public Transform player;
    public RandomPatrolWaypoints randomPatrolWaypoints;


    // Use this for initialization
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.speed = moveSpeed;
        myAgent.autoBraking = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (!myAgent.pathPending && myAgent.remainingDistance >= 0f)
        {
            {
                myAgent.SetDestination(player.transform.position);
                randomPatrolWaypoints.enabled = false;
            }
        }
    }

}