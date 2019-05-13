using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNew : MonoBehaviour
{
    public Vector3 startPos = new Vector3(0,0,0);
    public Vector3 endPos = new Vector3(0,0,0);
    public float moveSpeed = 1.0f;
    public Transform player;
    public Transform DoorA;
    public Transform DoorB;
    public float range = 10f;
    private bool openDoor = false;
    private bool doorAnimating;

    void Update()
    {
DoorCheck();
    }

    private void DoorCheck()
    {
        if (Vector3.Distance(player.position, this.transform.position) < range)
        {
            if (!doorAnimating && openDoor == false)
            {
                openDoor = true;
                //StopCoroutine("MoveDoor");
		        StartCoroutine(MoveDoors(DoorA, DoorB, endPos));
                
            }
        }
        else if (openDoor)
        {
            if (!doorAnimating)
            {
                StartCoroutine(MoveDoors(DoorA, DoorB, -endPos));
                openDoor = false;
            }
        }
    }


    IEnumerator MoveDoors (Transform door1, Transform door2, Vector3 dest) 
    {
        doorAnimating = true;
		float t = 0f;
        Vector3 dest1 = door1.position + dest;
        Vector3 dest2 = door2.position - dest;
        float dist = Vector3.Distance(door1.position, dest1);

        while (dist > 0.01f) {
            door1.position = Vector3.MoveTowards(door1.position, dest1, Time.deltaTime * moveSpeed);
            door2.position = Vector3.MoveTowards(door2.position, dest2, Time.deltaTime * moveSpeed);
            dist = Vector3.Distance(door1.position, dest1);
            yield return null;
        }
        doorAnimating = false;
	}
}
