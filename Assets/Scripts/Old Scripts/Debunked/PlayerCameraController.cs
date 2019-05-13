using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 5.0f;
    [SerializeField] private float smoothing = 0.2f;

    private GameObject player;
    private Vector2 smoothedVelocity;
    private Vector2 currentLookPos;

    private void Start()
    {
        player = transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        inputValues = Vector2.Scale(inputValues, new Vector2(mouseSensitivity * smoothing, mouseSensitivity * smoothing));
        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1.0f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1.0f / smoothing);

        currentLookPos += smoothedVelocity;

        transform.localRotation = Quaternion.AngleAxis(-currentLookPos.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(currentLookPos.x, player.transform.up);
    }
}
