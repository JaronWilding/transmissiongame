using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRun : MonoBehaviour
{

    public PlayerLook ascript;
    public PlayerMove bscript;
    private float timer = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        ascript = GetComponentInChildren<PlayerLook>();
        bscript = GetComponent<PlayerMove>();
        ascript.enabled = false;
        bscript.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Screen.lockCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0f)
        {
            ascript.enabled = true;
            bscript.enabled = true;
        }
    }
}
