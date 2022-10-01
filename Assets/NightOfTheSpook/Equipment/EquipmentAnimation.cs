using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentAnimation : MonoBehaviour
{
    public float AnimationDuration = 1.0f;
    public AnimationCurve YDisplacement;
    public AnimationCurve XZDisplacement;
    private Vector3 _startingScale;
    private float _elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        _startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        float pctComplete = _elapsedTime / AnimationDuration;

        transform.localScale = new Vector3(_startingScale.x * XZDisplacement.Evaluate(pctComplete),
            _startingScale.y * YDisplacement.Evaluate(pctComplete),
            _startingScale.z * XZDisplacement.Evaluate(pctComplete));
    }
}
