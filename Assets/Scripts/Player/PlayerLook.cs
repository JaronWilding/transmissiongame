using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    // Public Variables

    [Header("Input Variables")]
    [Space(1)]
    [Tooltip("Rotate on the X Axis")]
    [SerializeField] private string mouseXInputName = "Mouse X";
    [Tooltip("Rotate on the Y Axis")]
    [SerializeField] private string mouseYInputName = "Mouse Y";

    [Header("Look Properties")]
    [Space(1)]
    [Tooltip("Sensitivity of the mouse")]
    [Range(50, 300)]
    [SerializeField] private float mouseSensitivity = 150f;
    [Tooltip("The look-smoothing component")]
    [SerializeField] private float lookSmoothDamp = 0.1f;

    [Header("The Player")]
    [Tooltip("Place the Player here.")]
    [SerializeField] private Transform player;

    //Leaning Code
    public Transform bodyPivot;
    float curAngle = 0f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float maxAngle = 20f;

    // Private Variables

    private float currentYRotation;
    private float currentXRotation;
    private float yRotationV;
    private float xRotationV;
    private float xAxisClamp;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Screen.lockCursor = true;
        xAxisClamp = 0.0f;
        if (bodyPivot == null && transform.parent != null) bodyPivot = transform.parent;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            curAngle = Mathf.MoveTowardsAngle(curAngle, maxAngle, rotateSpeed * Time.deltaTime);
        }
        // lean right
        else if (Input.GetKey(KeyCode.E))
        {
            curAngle = Mathf.MoveTowardsAngle(curAngle, -maxAngle, rotateSpeed * Time.deltaTime);
        }
        // reset lean
        else
        {
            curAngle = Mathf.MoveTowardsAngle(curAngle, 0f, rotateSpeed * Time.deltaTime);
        }

        bodyPivot.transform.localRotation = Quaternion.AngleAxis(curAngle, Vector3.forward);
        //Every frame go to the Cam Rotation follow mouse
        CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if(xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        currentXRotation = Mathf.SmoothDamp(currentXRotation, mouseX, ref xRotationV, lookSmoothDamp);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, mouseY, ref yRotationV, lookSmoothDamp);

        transform.Rotate(Vector3.left * currentYRotation); //Camera's Y axis.
        player.Rotate(Vector3.up * currentXRotation); //Camera's X axis. This is attached to Player, so we rotate players body.
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
