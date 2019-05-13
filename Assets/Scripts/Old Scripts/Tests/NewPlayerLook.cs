using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerLook : MonoBehaviour
{
    //User Variables
    [Range(1.0f, 20.0f)]
    [SerializeField] private float lookSensitivity = 5.0f;
    [Range(0.01f, 1.0f)]
    [SerializeField] private float lookSmoothDamp = 0.1f;

    [SerializeField] private Vector2 minMaxRotation = new Vector2(-90, 90);

    [SerializeField] private string mouseX = "Mouse X";
    [SerializeField] private string mouseY = "Mouse Y";

    //Variables
    private float yRotation;
    private float xRotation;
    private float currentYRotation;
    private float currentXRotation;
    private float yRotationV;
    private float xRotationV;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //targetMain = GameObject.FindObjectOfType<Transform>();
    }

    private void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        //Gets the camera variables
        yRotation += Input.GetAxis(mouseX) * lookSensitivity;
        xRotation -= Input.GetAxis(mouseY) * lookSensitivity;

        //Clamps the camera
        xRotation = Mathf.Clamp(xRotation, minMaxRotation.x, minMaxRotation.y);

        //Smooths out the camera movement.
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);

        //Rotates camera completely
        transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
        transform.parent.eulerAngles = new Vector3(0, currentYRotation, 0);

    }
}
