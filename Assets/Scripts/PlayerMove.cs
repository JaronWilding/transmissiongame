using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    //Declare variables and set variables.

    private CharacterController charController;

    //Movement variables.
    [Header("Inputs")]
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    private float movementSpeed;

    //Run, walk, slow-walk variables.
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float runBuildUpSpeed;
    [SerializeField] private KeyCode runKey;

    //Jumping variables.
    [Header("Jumping Variables")]
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    //Still to do with jumping, but more so that no stepping occurs.
    [SerializeField] private float slopeForce, slopeForceRaylength;
    private bool isJumping;

    //Crouching variables.
    [Header("Crouching Variables")]
    [SerializeField] private float controllerHeight;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float crouchSpeedMult;
    [SerializeField] private KeyCode crouchKey;
    [SerializeField] private Camera cameraMain;
    [SerializeField] private bool HoldKey;

    private bool isCrouching;
    private bool isRunning;

    private delegate void CrouchModeDelegate();
    private CrouchModeDelegate setCrouch;

    
    private void Awake()
    {
        charController = GetComponent<CharacterController>();

        if (HoldKey) // Hold mode
            setCrouch = CrouchInputHold;
        else // Toggle mode
            setCrouch = CrouchInputToggle;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        //Get the players input.
        // * Time.deltaTime <= see charController.SimpleMove comment as to why deltaTime was not used
        float horizInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName); 
        
        //Create a Vector3 for all axis.
        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        if ((vertInput != 0 || horizInput != 0) && onSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);

        //SimpleMove applies Time.DeltaTime under the hood.
        //Clamping the magnitude and multiplying it by the move speed. Means moving diagonally does not make the player faster.
        //We multiply the magnitude otherwise the clamp would clamp our speed.
        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        setCrouch();
        SetMovementSpeed();
        JumpInput();
        FOV();
    }


    //Sprinting
    private void SetMovementSpeed()
    {
        if (Input.GetKey(runKey)) {
            if (isCrouching)
                isCrouching = false;
            SetLocalCameraY();
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        }
        else
            if (isCrouching)
                movementSpeed = Mathf.Lerp(movementSpeed, crouchSpeed, Time.deltaTime * runBuildUpSpeed);
            else
                movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
                
    }

    private void FOV()
    {
        Vector3 charSpeedVec = charController.velocity;
        charSpeedVec.y = 0.0f;
        float charSpeed = charSpeedVec.magnitude;

        if (charSpeed <= 6)
            cameraMain.fieldOfView = 60.0f;
        else if (charSpeed >= 9)
            cameraMain.fieldOfView = 80.0f;
        else
            cameraMain.fieldOfView = 60.0f + (20.0f / 3.0f * (charSpeed - 6.0f));
    }

    #region CrouchEvents
    /// Modifies the local camera Y position.
    private void SetLocalCameraY()
    {
        if (isCrouching) // Transform to crouching height
            cameraMain.transform.localPosition = new Vector3(0, Mathf.Lerp(cameraMain.transform.localPosition.y, crouchHeight, Time.deltaTime * crouchSpeedMult), 0);
        else // Transform to standing height
            cameraMain.transform.localPosition = new Vector3(0, Mathf.Lerp(cameraMain.transform.localPosition.y, controllerHeight, Time.deltaTime * crouchSpeedMult), 0);
    }

    /// Sets the crouching modifier when Toggle-Crouch mode is selected.
    private void CrouchInputToggle()
    {
        // Invert crouch state on key press
        if (Input.GetKeyDown(crouchKey))
            isCrouching = !isCrouching;

        SetLocalCameraY();
    }

    /// Sets the crouching modifier when Hold-Crouch mode is selected.
    private void CrouchInputHold()
    {
        // Crouching -> Standing
        if (isCrouching && !Input.GetKey(crouchKey))
            isCrouching = false;
        // Standing -> Crouching
        else if (!isCrouching && Input.GetKey(crouchKey) && !Input.GetKey(runKey))
            isCrouching = true;

        SetLocalCameraY();
    }

    #endregion

    #region JumpEvents
    //gets the jump input
    private void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            isCrouching = false;
            SetLocalCameraY();
            StartCoroutine(JumpEvent());
        }
    }

    //Jump event.
    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;

            yield return null;

        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);


        charController.slopeLimit = 45.0f;
        isJumping = false;

    }

    //This makes sure no stepping occurs when going down ramps.
    private bool onSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRaylength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }

#endregion Jump Events

}
