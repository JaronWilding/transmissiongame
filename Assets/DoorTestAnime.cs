using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTestAnime : MonoBehaviour
{

    [SerializeField] private GameObject DoorHalfBig;
    [SerializeField] private GameObject DoorHalfSmall;
    [SerializeField] private string DoorHalfBigState;
    [SerializeField] private string DoorHalfSmallState;

    private Animator animDoorHalfBig;
    private Animator animDoorHalfSmall;

    private bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        animDoorHalfBig = DoorHalfBig.GetComponentInChildren<Animator>();
        animDoorHalfSmall = DoorHalfSmall.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DoorOpen();
        //AnimatorStateInfo info = animDoorHalfBig.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(info.length);
    }

    private void DoorOpen()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //AnimatorStateInfo info = 
            bool a = animDoorHalfBig.GetCurrentAnimatorStateInfo(0).IsName(DoorHalfBigState);
            //Debug.Log(info.IsName(DoorHalfBigState));
            Debug.Log("F pressed");
            isOpen = !isOpen;
            if (isOpen)
            {
                animDoorHalfBig.SetFloat("Speed", 1f);
                animDoorHalfBig.Play(DoorHalfBigState, -1, 0f);
                animDoorHalfSmall.SetFloat("Speed", 1f);
                animDoorHalfSmall.Play(DoorHalfSmallState, -1, 0f);
                //animDoorHalfBig.SetTrigger("Reset");
            }
            else
            {
                animDoorHalfBig.SetFloat("Speed", -1f);
                animDoorHalfBig.Play(DoorHalfBigState, -1, 1f);
                animDoorHalfSmall.SetFloat("Speed", -1f);
                animDoorHalfSmall.Play(DoorHalfSmallState, -1, 1f);
                animDoorHalfBig.SetTrigger(1);
            }
        }
    }
}
