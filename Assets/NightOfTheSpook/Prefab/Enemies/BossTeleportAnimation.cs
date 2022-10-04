using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleportAnimation : MonoBehaviour
{
    [Range(0, 5.0f)]
    public float duration;

    public AnimationCurve scaleCurve;

    private Vector3 _startingScale;
    private float _deltaTime;
    // Start is called before the first frame update

    /// <summary>
    /// 
    /// </summary>
    public delegate void CompletionHandler();

    /// <summary>
    /// Callback for after the animation is finished.
    /// </summary>
    public CompletionHandler OnAnimationFinished;

    void Start()
    {
        _startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime += Time.deltaTime;

        float pct = _deltaTime / duration;

        

        if (pct >= 1.0)
        {
            OnAnimationFinished();
            Destroy(this);
        } else {
            transform.localScale = _startingScale * scaleCurve.Evaluate(pct);
        }
    }
}