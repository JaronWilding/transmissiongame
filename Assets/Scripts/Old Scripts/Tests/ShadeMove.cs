using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShadeMove : MonoBehaviour
{

    #region Variables

    [Header("Player Options")]
    [SerializeField] private float playerHeight;

    [Header("Movement Options")]
    [Range(1.0f, 20.0f)]
    [SerializeField] private float movementSpeed = 8.0f;
    [SerializeField] private bool smooth;
    [SerializeField] private float smoothSpeed;

    [Header("Jump Options")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpDecrease;

    [Header("Gravity")]
    [SerializeField] private float gravity = 2.5f;

    [Header("Physics")]
    [SerializeField] private LayerMask discludePlayer;

    [Header("References")]

    [SerializeField] private SphereCollider sphereCol;



    //Private Variables

    //Movement Vectors
    private Vector3 velocity;
    private Vector3 move;

    //Gravity Variables
    private bool grounded;
    private float currentGravity = 0.0f;
    private Vector3 liftPoint = new Vector3(0, 1.2f, 0);
    private RaycastHit groundHit;
    private Vector3 groundCheckPoint = new Vector3(0, -0.87f, 0);

    //Jump Variables
    private float jumpHeight = 0f;
    private bool inputJump = false;


    #endregion

    #region Main Methods

    private void Update()
    {
        Gravity();
        SimpleMove();
        Jump();
        FinalMove();
        GroundChecking();
        CollisionCheck();
    }

    #endregion


    #region Movement Methods

    private void SimpleMove()
    {
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        velocity += move;
    }

    private void FinalMove()
    {
        Vector3 vel = velocity * movementSpeed;
        //velocity = new Vector3(move.x, -currentGravity + jumpHeight, move.z) * movementSpeed;
        //velocity = transform.TransformDirection(velocity);
        vel = transform.TransformDirection(vel);

        transform.position += vel * Time.deltaTime;

        velocity = Vector3.zero;
    }

    #endregion


    #region Grounding & Gravity

    private void Gravity()
    {
        if(grounded == false)
        {
            //currentGravity = gravity;
            velocity.y -= gravity;
        }
        else
        {
            //currentGravity = 0.0f;
        }
    }

    private void GroundChecking()
    {
        Ray ray = new Ray(transform.TransformPoint(liftPoint), Vector3.down);
        //Debug.DrawRay(transform.TransformPoint(liftPoint), Vector3.down, Color.red);

        RaycastHit tempHit = new RaycastHit();

        
        if(Physics.SphereCast(ray, 0.17f, out tempHit, 20, discludePlayer))
        {
            GroundConfirm(tempHit);
        }
        else
        {
            grounded = false;
        }
    }

    private void GroundConfirm(RaycastHit tempHit)
    {
        //float currentSlope = Vector3.Angle(tempHit.normal, Vector3.up);

        Collider[] col = new Collider[3];
        int num = Physics.OverlapSphereNonAlloc(transform.TransformPoint(groundCheckPoint), 0.57f, col, discludePlayer);

        grounded = false;

        for(int i = 0; i < num; i++)
        {
            if (col[i].transform == tempHit.transform)
            {
                groundHit = tempHit;
                grounded = true;

                //Snapping
                if (inputJump == false && jumpHeight == 0f)
                {
                    //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, (groundHit.point.y + (playerHeight / 2)), transform.position.z), 2.5f);
                    transform.position = new Vector3(transform.position.x, (groundHit.point.y + (playerHeight / 2)), transform.position.z);
                }
                break;
            }
        }

        //Check gravity
        if(num <= 1 && tempHit.distance <= 3.1f && inputJump == false)
        {
            if(col[0] != null)
            {
                Ray ray = new Ray(transform.TransformPoint(liftPoint), Vector3.down);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit, 3.1f, discludePlayer))
                {
                    if(hit.transform != col[0].transform)
                    {
                        grounded = false;
                        return;
                    }
                }
            }
        }

    }

    #endregion


    #region Collision Check

    private void CollisionCheck()
    {
        Collider[] overlaps = new Collider[4];
        int num = Physics.OverlapSphereNonAlloc(transform.TransformPoint(sphereCol.center), sphereCol.radius, overlaps, discludePlayer, QueryTriggerInteraction.UseGlobal);

        for(int i = 0; i < num; i++)
        {
            Transform t = overlaps[i].transform;
            Vector3 dir;
            float dist;

            if (Physics.ComputePenetration(sphereCol, transform.position, transform.rotation, overlaps[i], t.position, t.rotation, out dir, out dist))
            {
                Vector3 penetrationVector = dir * dist;
                Vector3 velocityProject = Vector3.Project(velocity, -dir);
                transform.position = transform.position + penetrationVector;
                velocity -= velocityProject;
            }

        }
    }

    #endregion


    #region Jumping

    private void Jump()
    {
        bool canJump = false;
        canJump = !Physics.Raycast(new Ray(transform.position, Vector3.up), playerHeight, discludePlayer);

        if (grounded && jumpHeight > 0.2f || jumpHeight <= 0.6f && grounded)
        {
            jumpHeight = 0;
            inputJump = false;
        }



        if(grounded && canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                inputJump = true;
                transform.position += Vector3.up * 0.15f * 2;
                jumpHeight += jumpForce;
            }
        }
        else
        {
            if (!grounded)
            {
                jumpHeight -= (jumpHeight * jumpDecrease * Time.deltaTime);
            }
        }

        velocity.y += Mathf.Max(0, jumpHeight);

    }

    #endregion


}
