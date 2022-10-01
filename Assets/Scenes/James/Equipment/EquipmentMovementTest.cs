using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentMovementTest : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float speed;

    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;

        Vector3 velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            velocity += cameraForward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += -cameraRight;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity += -cameraForward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += cameraRight;
        }

        _rb.velocity = velocity * speed;
    }
}
