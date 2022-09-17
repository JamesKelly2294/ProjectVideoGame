using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class ButtonHighlightExpand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isText;
    public float scaleFactor = 1.0f;
    public float duration = 1.0f;

    private TextMeshProUGUI _text;
    private float _originalFontSize;
    private TweenerCore<float, float, FloatOptions> _growthAnimation;
    
    void OnEnable()
    {
        if (isText)
        {
            _text = GetComponent<TextMeshProUGUI>();
            _originalFontSize = _text.fontSize;
        }
    }

    void OnDisable()
    {
        if (isText)
        {
            _text.fontSize = _originalFontSize;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isText)
        {
            _text.fontSize = _originalFontSize;
            _growthAnimation = _text.DOFontSize(_originalFontSize * scaleFactor, duration);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isText)
        {
            _growthAnimation.Kill();
            _text.DOFontSize(_originalFontSize, duration);
        }
    }
}
