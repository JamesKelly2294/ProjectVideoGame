using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public float progress = 0.25f;
    public bool isIndeterminate = true;
    public float indeterminateTime = 0f;
    public AnimationCurve indeterminateCurve;
    public float indeterminateDuration = 1f;

    public RectTransform progressIndicatorContainer;
    public LayoutElement progressIndicator, progressIndicatorLeftPadding;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var width = progressIndicatorContainer.sizeDelta.x;
        var barSize = Mathf.Max(width * progress, progressIndicator.minWidth);

        // Create the wiggle waggle of the indeterminate mode
        var leftPaddingSize = 0f;
        if (( indeterminateTime > 0 && indeterminateTime < 2 ) || isIndeterminate) {
            var paddingMax = width - barSize;
            if (indeterminateTime > 2) { indeterminateTime -= 2; }
            indeterminateTime += (Time.deltaTime / indeterminateDuration);
            var indeterminateProgress = indeterminateCurve.Evaluate(indeterminateTime);
            leftPaddingSize = paddingMax * indeterminateProgress;
        } else {
            indeterminateTime = 0f;
        }

        progressIndicator.preferredWidth = barSize;
        progressIndicatorLeftPadding.preferredWidth = leftPaddingSize;
    }

    public void SetIndeterminateMode(bool isIndeterminate)
    {
        this.isIndeterminate = isIndeterminate;
    }

    public void SetProgress(float progress)
    {
        this.progress = Mathf.Clamp01(progress);
    }
}
