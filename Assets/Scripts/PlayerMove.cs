using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Transform cameraMain;
    [SerializeField] private bool HoldKey;

    [Header("Switch Cameras")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private Camera securityCam;
    [SerializeField] private Camera switchCam;
    [SerializeField] private KeyCode switchCamKey;

    // THIS IS NEW. You may want to seralize this if the environment can enforce crouching. 
    private bool isCrouching;
    private float startTime;
    public float speed = 1.0F;
    
    // A note here, the way I've got it crouch mode is only set when awake is called:
    // If you want it changed from a menu during gameplay, you'll either need to move it
    // set it externally, might be a pain. If you need to do that, buzz me. Should be able
    // to use a const function (compile time fn) with both delegates already made and just have
    // the menu change them.
    private delegate void CrouchModeDelegate();
    private CrouchModeDelegate setCrouch;


    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        if (HoldKey) // Hold mode
            setCrouch = CrouchInputHold;
        else // Toggle mode
            setCrouch = CrouchInputToggle;

        playerCam.enabled = true;
        securityCam.enabled = false;

    }

    private void Update()
    {
        PlayerMovement();


        if (Input.GetKeyDown(switchCamKey)) {
            if(playerCam.enabled){
                
                camChange();
                
            }else{
                camChange_B();
            }
            //switchCam.enabled = !switchCam.enabled;
        }
    }

    private void camChange(){
        playerCam.enabled = false;

        switchCam.transform.position = playerCam.transform.position;
        Vector3 pos = new Vector3();
        pos = securityCam.transform.position;

        switchCam.enabled = true;
        switchCam.transform.position = Vector3.Lerp(switchCam.transform.position, pos, Time.deltaTime);
        switchCam.enabled = false;

        securityCam.enabled = true;
    }
    private void camChange_B(){
        securityCam.enabled = false;

        switchCam.transform.position = securityCam.transform.position;
        Vector3 pos = new Vector3();
        pos = playerCam.transform.position;

        
        switchCam.enabled = true;

        float journeyLength;
        journeyLength = Vector3.Distance(securityCam.transform.position, pos);
        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / journeyLength;


        switchCam.transform.position = Vector3.Lerp(switchCam.transform.position, pos, fracJourney);
        switchCam.enabled = false;
        playerCam.enabled = true;
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


        //SimpleMove applies Time.DeltaTime under the hood.
        //Clamping the magnitude and multiplying it by the move speed. Means moving diagonally does not make the player faster.
        //We multiply the magnitude otherwise the clamp would clamp our speed.
        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        if ((vertInput != 0 || horizInput != 0) && onSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);

        setCrouch();
        SetLocalCameraY();
        SetMovementSpeed();
        JumpInput();
        

    }


    //Sprinting
    private void SetMovementSpeed()
    {
        if (Input.GetKey(runKey))
            movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, Time.deltaTime * runBuildUpSpeed);
        else
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * runBuildUpSpeed);
    }


    /// <summary>
    /// Modifies the local camera Y position.
    /// </summary>
    private void SetLocalCameraY()
    {
        if (isCrouching) // Transform to crouching height
        {
            cameraMain.transform.localPosition = new Vector3(0, Mathf.Lerp(
                cameraMain.transform.localPosition.y,
                crouchHeight,
                Time.deltaTime * crouchSpeedMult), 0);

            //charController.height = Mathf.Lerp(charController.height, crouchHeight, Time.deltaTime * crouchSpeedMult);
        }
        else // Transform to standing height
        {
            cameraMain.transform.localPosition = new Vector3(0, Mathf.Lerp(
                cameraMain.transform.localPosition.y,
                controllerHeight,
                Time.deltaTime * crouchSpeedMult), 0);

            //charController.height = Mathf.Lerp(charController.height, controllerHeight, Time.deltaTime * crouchSpeedMult);
        }
    }

    /// <summary>
    /// Sets the crouching modifier when Toggle-Crouch mode is selected.
    /// </summary>
    private void CrouchInputToggle()
    {
        // Invert crouch state on key press
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = !isCrouching;
        }
    }

    /// <summary>
    /// Sets the crouching modifier when Hold-Crouch mode is selected.
    /// </summary>
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
        
    }



    //gets the jump input
    private void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
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

}
