using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControllerWithLean : MonoBehaviour
{
    #region Variables

    //Movement variables.
    [Header("Inputs")]
    [SerializeField] private string horizontalInputName = "Horizontal";
    [SerializeField] private string verticalInputName = "Vertical";
    private float movementSpeed;

    //Run, walk, slow-walk variables.
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float runSpeed = 9.0f;
    [SerializeField] private float runBuildUpSpeed = 2.0f;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    //Camera rotate

    public Transform _Pivot;
    float curAngle = -4f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float maxAngle = 20f;



   //Jumping variables.
   [Header("Jumping Variables")]
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier = 10.0f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    //Still to do with jumping, but more so that no stepping occurs.
    [SerializeField] private float slopeForce = 5.0f;
    [SerializeField] private float slopeForceRaylength = 1.5f;
    private bool isJumping;

    //Crouching variables.
    [Header("Crouching Variables")]
    [SerializeField] private float controllerHeight = 0.9f;
    [SerializeField] private float crouchHeight = 0.1f;
    [SerializeField] private float crouchSpeedMult = 5.0f;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private Camera cameraMain;
    [SerializeField] private bool HoldKey = false;

    [SerializeField] private float fovSpeed;


    private CharacterController charController;
    public bool isCrouching { get; private set; }
    public bool isRunning { get; private set; }
    private Vector3 lastVector;



    private delegate void CrouchModeDelegate();
    private CrouchModeDelegate setCrouch;

    #endregion


    private void Awake()
    {
        if (_Pivot == null && transform.parent != null) _Pivot = transform.parent;

        charController = GetComponent<CharacterController>();
        if (HoldKey) // Hold mode
        {
            setCrouch = CrouchInputHold;
        }
        else // Toggle mode
        {
            setCrouch = CrouchInputToggle;
        }
        
         
        
    }

    private void Update()
    {
        

            // lean left
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

            _Pivot.transform.localRotation = Quaternion.AngleAxis(curAngle, Vector3.forward);
        


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

        // Pushes our character controller down so it'll be grounded.
        if ((vertInput != 0 || horizInput != 0) && onSlope())
        {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
        }

        //SimpleMove applies Time.DeltaTime under the hood.
        //Clamping the magnitude and multiplying it by the move speed. Means moving diagonally does not make the player faster.
        //We multiply the magnitude otherwise the clamp would clamp our speed.
        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        setCrouch();
        SetMovementSpeed();
        JumpInput();
    }


    //Sprinting
    private void SetMovementSpeed()
    {
        if (Input.GetKey(runKey))
        {
            if (isCrouching)
            {
                isCrouching = false;
            }
            SetLocalCameraY();
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        }
        else
            if (isCrouching)
        {
            movementSpeed = Mathf.Lerp(movementSpeed, crouchSpeed, Time.deltaTime * runBuildUpSpeed);
        }
        else
        {
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
        }

    }

    // On Hold
    private void FOV()
    {
        Vector3 charSpeedVec = (charController.transform.position - lastVector) * Time.deltaTime;//charController.velocity;
        charSpeedVec.y = 0.0f;
        float charSpeed = charSpeedVec.magnitude * fovSpeed;

        cameraMain.fieldOfView = 60.0f * scale(0.0f, 10.0f, 1.0f, 1.2f, charSpeed);
    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    #region CrouchEvents
    /// Modifies the local camera Y position.
    private void SetLocalCameraY()
    {
        if (isCrouching) // Transform to crouching height
        {
            cameraMain.transform.localPosition = new Vector3(0, Mathf.Lerp(cameraMain.transform.localPosition.y, crouchHeight, Time.deltaTime * crouchSpeedMult), 0);
            charController.height = Mathf.Lerp(charController.height, 0.4f, 5.0f * Time.deltaTime);
            //charController.transform.position += new Vector3(0, (( charController.height - 2f ) * 0.5f), 0); 
        }

        else // Transform to standing height
        {
            cameraMain.transform.localPosition = new Vector3(0, Mathf.Lerp(cameraMain.transform.localPosition.y, controllerHeight, Time.deltaTime * crouchSpeedMult), 0);
            charController.height = Mathf.Lerp(charController.height, 2f, 5.0f * Time.deltaTime);
            //charController.transform.position += new Vector3(0, (( charController.height - 1f ) * 0.5f), 0); 
        }
    }

    /// Sets the crouching modifier when Toggle-Crouch mode is selected.
    private void CrouchInputToggle()
    {
        // Invert crouch state on key press
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = !isCrouching;
        }
        SetLocalCameraY();
    }

    /// Sets the crouching modifier when Hold-Crouch mode is selected.
    private void CrouchInputHold()
    {
        // Crouching -> Standing
        if (isCrouching && !Input.GetKey(crouchKey))
        {
            isCrouching = false;
        }
        // Standing -> Crouching
        else if (!isCrouching && Input.GetKey(crouchKey) && !Input.GetKey(runKey))
        {
            isCrouching = true;
        }

        SetLocalCameraY();
    }

    #endregion

    #region JumpEvents
    //gets the jump input
    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            if (isCrouching)
            {
                isCrouching = false;
            }
            isJumping = true;
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
        {
            return false;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRaylength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    #endregion Jump Events

}
