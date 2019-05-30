using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRun : MonoBehaviour
{

    private PlayerLook pLook;
    private PlayerMove pMove;
    private OnRun onRunScript;
    private float timer = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        pLook = GetComponentInChildren<PlayerLook>();
        pMove = GetComponent<PlayerMove>();
        onRunScript = GetComponent<OnRun>();
        pLook.enabled = false;
        pMove.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; //hides mouse cursor during main build
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0f)
        {
            pLook.enabled = true;
            pMove.enabled = true;
            onRunScript.enabled = false;
        }
    }

}
