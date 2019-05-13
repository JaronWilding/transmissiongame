using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 5.0f;
    [SerializeField] private float distFromTarget = 0.0f;
    [SerializeField] private Vector2 pitchMinMax = new Vector2(-40, 85);
    [SerializeField] private float rotationSmoothTime = 0.12f;
    [SerializeField] private Transform target;

    private bool lockCursor;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;
    private float yaw;
    private float pitch;

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        Vector3 e = transform.eulerAngles;
        e.x = 0;

        target.eulerAngles = e;
        transform.position = target.position - transform.forward * distFromTarget;
    }

}
