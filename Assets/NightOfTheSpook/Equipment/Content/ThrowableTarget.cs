using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableTarget : MonoBehaviour
{
    public GameObject target;

    [Range(0.0f, 10.0f)]
    public float ParabolaHeight = 5.0f;
    [Range(0.0f, 10.0f)]
    public float ParabolaDuration = 5.0f;

    private Vector3 _start;
    private Vector3 _end;
    private Vector3 _prevPosition;

    public Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, (-4 * height * t * t + 4 * height * t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    float _elapsedTime;

    public bool ThrowLanded
    {
        get
        {
            return _elapsedTime > ParabolaDuration;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _start = transform.position;
        _end = target.transform.position;
        _prevPosition = _start;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        float pct = Mathf.Clamp01(_elapsedTime / ParabolaDuration);

        _prevPosition = transform.position;
        transform.position = Parabola(_start, _end, ParabolaHeight, pct);
    }

    public Vector3 DirectionOfTravel
    {
        get
        {
            var dir = (_end - _start).normalized;
            return new Vector3(dir.x, 0.0f, dir.z);
        }
    }

    private void FixedUpdate()
    {
        var direction = (_prevPosition - transform.position).normalized;
        transform.LookAt(transform.position + (direction * 10));
    }
}
