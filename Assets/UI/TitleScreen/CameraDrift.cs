using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrift : MonoBehaviour
{
    public Vector2 min;
    public Vector2 max;
    [Range(0.01f, 10f)]
    public float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    Vector3 _newPositon;
    Vector3 _initialPosition;
    void Start()
    {
        _initialPosition = transform.position;
        _newPositon = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _newPositon, Time.deltaTime * lerpSpeed);
        
        if(Vector3.Distance(transform.position, _newPositon) < 0.001)
        {
            GetNewPosition();
        }
    }

    void GetNewPosition()
    {
        var xPos = Random.Range(min.x, max.x);
        var yPos = Random.Range(min.y, max.y);
        _newPositon = new Vector3(xPos, yPos, _initialPosition.z);
    }
}
