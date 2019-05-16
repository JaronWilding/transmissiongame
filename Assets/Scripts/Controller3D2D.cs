using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller3D2D : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        transform.position += v * Vector3.forward * Time.deltaTime * moveSpeed;
        transform.position += h * Vector3.right * Time.deltaTime * moveSpeed;
        
    }
}
