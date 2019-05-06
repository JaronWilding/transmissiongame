using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerLook : MonoBehaviour
{
    //User Variables
    [SerializeField] private float lookSensitivity = 5.0f;
    [SerializeField] private float lookSmoothDamp = 0.1f;

    //Variables
    private float yRotation;
    private float xRotation;
    public static float currentYRotation;
    private float currentXRotation;
    private float yRotationV;
    private float xRotationV;

    
    void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        //Gets the camera variables
        yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;

        //Clamps the camera
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //Smooths out the camera movement.
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);
        

        //Rotates camera completely
        transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
    }
}
