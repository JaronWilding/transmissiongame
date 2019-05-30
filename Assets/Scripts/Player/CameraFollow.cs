using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{


    [SerializeField] public Transform headTrans;
    // Start is called before the first frame update
    void Start()
    {
        headTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left * PlayerLook.currentYRotation);
    }
}
