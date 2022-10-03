using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentDestructionAnimation : MonoBehaviour
{
    [Range(0, 5.0f)]
    public float duration;

    public AnimationCurve scaleCurve;

    private Vector3 _startingScale;
    private float _deltaTime;
    // Start is called before the first frame update
    void Start()
    {
        _startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime += Time.deltaTime;

        float pct = _deltaTime / duration;

        transform.localScale = _startingScale * scaleCurve.Evaluate(pct);

        if (pct >= 1.0)
        {
            Destroy(gameObject);
        }
    }
}
