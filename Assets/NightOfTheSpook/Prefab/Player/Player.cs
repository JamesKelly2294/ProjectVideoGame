using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 10f;
    public GameObject inner;
    Plane plane = new Plane(Vector3.up, 0);

    public AnimationCurve walkVertical;
    public AnimationCurve walkWobble;
    public float walkVerticalTime, walkVerticalTotalTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (Time.timeScale == 0) { return; }

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

        movement = Quaternion.Euler(0, -45, 0) * movement.normalized;
        transform.position += movement * Time.deltaTime * speed;

        // Animate walk
        if (movement != Vector3.zero) {
            walkVerticalTime += Time.deltaTime;
            if (walkVerticalTime > walkVerticalTotalTime) {
                walkVerticalTime -= walkVerticalTotalTime;
            }
        } else {
            // If we are beyond half way, just continue
            if (walkVerticalTime > walkVerticalTotalTime / 2 && walkVerticalTime < walkVerticalTotalTime) {
                walkVerticalTime += Time.deltaTime;
                walkVerticalTime = Mathf.Min(walkVerticalTotalTime, walkVerticalTime);
            } else if (walkVerticalTime < walkVerticalTotalTime / 2 && walkVerticalTime > 0) {
                walkVerticalTime -= Time.deltaTime;
                walkVerticalTime = Mathf.Max(0, walkVerticalTime);
            }
        }
        float innerVerticalOffet = walkVertical.Evaluate(walkVerticalTime / walkVerticalTotalTime);
        inner.transform.position = new Vector3(inner.transform.position.x, innerVerticalOffet, inner.transform.position.z);

        float distance;
        Vector3 cursorLoc = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            cursorLoc = ray.GetPoint(distance);
        }

        cursorLoc = new Vector3(cursorLoc.x, inner.transform.position.y, cursorLoc.z);
        inner.transform.LookAt(cursorLoc);
        Vector3 eulerRotation = inner.transform.rotation.eulerAngles;
        inner.transform.rotation = Quaternion.Euler(new Vector3(eulerRotation.x, eulerRotation.y, walkWobble.Evaluate(walkVerticalTime / walkVerticalTotalTime) * 90));
    }
}
