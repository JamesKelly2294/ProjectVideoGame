using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class MoveAndFadeText : MonoBehaviour
{
    public bool destroyOnCompletion;

    public Vector3 direction;
    public float distance;
    public float duration;

    public AnimationCurve movementCurve;
    public AnimationCurve fadeoutCurve;

    private Vector3 _startPosition;
    private float _startTime;

    private bool _startPositionInitialized;
    private bool _started;

    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void TriggerAnimation()
    {
        if (!_startPositionInitialized)
        {
            _startPosition = transform.position;
            _startPositionInitialized = true;
        }
        else
        {
            transform.position = _startPosition;
        }
        _startTime = Time.time;

        _started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_started) { return; }

        var elaspedTime = Time.time - _startTime;
        var pctComplete = elaspedTime / duration;
        var normalizedDirection = direction.normalized;
        transform.position = _startPosition + normalizedDirection * distance * movementCurve.Evaluate(pctComplete);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1-fadeoutCurve.Evaluate(pctComplete));

        if (pctComplete >= 1.0)
        {
            if (destroyOnCompletion)
            {
                Destroy(gameObject);
            }
            else
            {
                _started = false;
            }
        }
    }
}
