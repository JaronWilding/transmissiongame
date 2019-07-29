using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRaycastOnAndOff : MonoBehaviour
{

    public RayCastForPlayer rayCastForPlayer;


    void Start()
    {
        
    }

    void Update()
    {

    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("GameController"))
        {
            rayCastForPlayer.enabled = true;
           
        }

    }
    private void OnTriggerExit(Collider col)
    {
        rayCastForPlayer.enabled = false;

    }
}
