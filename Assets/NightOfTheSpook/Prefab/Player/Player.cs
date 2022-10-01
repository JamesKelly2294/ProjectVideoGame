using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 10f;
    public GameObject inner;
    Plane plane = new Plane(Vector3.up, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 movement = Vector3.zero;
        if (Input.GetKey("w"))
        {
            movement += Vector3.forward;
        }

        if (Input.GetKey("s"))
        {
            movement += Vector3.back;
        }

        if (Input.GetKey("a"))
        {
            movement += Vector3.left;
        }

        if (Input.GetKey("d"))
        {
            movement += Vector3.right;
        }

        movement = movement.normalized;
        transform.position += movement * Time.deltaTime * speed;

        float distance;
        Vector3 cursorLoc = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            cursorLoc = ray.GetPoint(distance);
        }

        inner.transform.LookAt(cursorLoc);
    }
}
