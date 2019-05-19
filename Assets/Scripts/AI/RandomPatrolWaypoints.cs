using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPatrolWaypoints : MonoBehaviour
{
    NavMeshAgent myAgent;
    public Transform[] points;
    public int destinationPoint = 0;
    public float patrolSpeed = 7f;
    private int waypointInd;
    public GameObject[] waypoints;
    public GameObject[] player;
 //   public RunToExit runToExit;
    

    public float escapeTime = 0f;
    public float timerRange;






    // Use this for initialization
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.speed = patrolSpeed;
        myAgent.autoBraking = true;

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        player = GameObject.FindGameObjectsWithTag("Player");
        waypointInd = Random.Range(0, waypoints.Length);

        StartCoroutine("ExitTimer");


        timerRange = Random.Range(60.0f, 90.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!myAgent.pathPending && myAgent.remainingDistance <= 0.5f)
        {
            NextPoint();
        }
         if (escapeTime >= timerRange)
        {
            StopCoroutine("ExitTimer");
   //         runToExit.enabled = true;
            
        }
    
    }

    void NextPoint()
    {
        myAgent.speed = patrolSpeed;
        if (Vector3.Distance(transform.position, waypoints[waypointInd].transform.position) >= 2)
        {
            myAgent.SetDestination(waypoints[waypointInd].transform.position);
            
        }
        else if (Vector3.Distance(transform.position, waypoints[waypointInd].transform.position) <= 2)
        {
            waypointInd = Random.Range(0, waypoints.Length);
        }
       
    }

    IEnumerator ExitTimer()
{
    while (true)
    {
        yield return new WaitForSeconds(1);
        escapeTime++;

    }
}

}



