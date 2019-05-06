using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBob : MonoBehaviour
{
    private Camera cam;
    private PlayerMove PlayerMove;
    private CharacterController CController;
    

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        CController = gameObject.GetComponentInParent<CharacterController>();


    }

    // Update is called once per frame
    void Update()
    {
        Bobbing();
    }
    private void Bobbing()
    {

        Vector3 newCamPos;
    }



}