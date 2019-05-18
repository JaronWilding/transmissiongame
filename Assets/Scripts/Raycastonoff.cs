using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycastonoff : MonoBehaviour
{

    public RayCastforPlayer rayCastForPlayer;
    // public RandomPatrolWaypoints RandomPatrolWaypoints;
    // public RunToExit RunToExit;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            rayCastForPlayer.enabled = true;
            //  RandomPatrolWaypoints.enabled = false;
            //  RunToExit.enabled = true;
        }

    }
    private void OnTriggerExit(Collider col)
    {
        rayCastForPlayer.enabled = false;

    }
}
