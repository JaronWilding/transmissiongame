using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    //Inspector Variables
    [Header("Moving variables")]
    [Range(0.5f, 6.0f)]
    [SerializeField] private float moveAmount = 0.9f;
    [Range(1.0f, 8.0f)]
    [SerializeField] private float moveSpeed = 3.0f;
    [Range(1.1f, 5.0f)]
    [SerializeField] private float range = 2.0f;
    [Header("Transform variables")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject DoorA_GO;
    [SerializeField] private GameObject DoorB_GO;

    //Private variables
    private bool openDoor = false;  
    private bool doorAnimating;
    private Vector3 endPos = new Vector3(0,0,0);
    private Transform DoorA;
    private Transform DoorB;

    private void Start()
    {
        SetAxis(DoorA_GO);
        DoorA = DoorA_GO.GetComponent<Transform>();
        DoorB = DoorB_GO.GetComponent<Transform>();
    }
    void Update()
    {
        DoorCheck();
    }


    private void SetAxis(GameObject Input)
    {
        Vector3 size = DoorA_GO.GetComponent<Renderer>().bounds.size;
        int axis = 0;
        float sizeMax = Mathf.Max(size.x, size.y, size.z);
        float sizeMin = Mathf.Min(size.x, size.y, size.z);

        for(int i = 0; i < 3; i++)
        {
            if(size[i] < sizeMax && size[i] > sizeMin){
                axis = i;
            }
        }
        endPos[axis] = moveAmount;
    }


    private void DoorCheck()
    {
        if (Vector3.Distance(player.position, this.transform.position) < range)
        {
            if (!doorAnimating && openDoor == false)
            {
                openDoor = true;
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
