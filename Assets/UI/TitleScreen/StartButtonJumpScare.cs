using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartButtonJumpScare : MonoBehaviour, IPointerEnterHandler
{
    // public bool isText;
    // public float scaleFactor = 1.0f;
    // public float duration = 1.0f;

    // private TextMeshProUGUI _text;
    // private float _originalFontSize;
    // private TweenerCore<float, float, FloatOptions> _growthAnimation;
    
    // void OnEnable()
    // {
    //     if (isText)
    //     {
    //         _text = GetComponent<TextMeshProUGUI>();
    //         _originalFontSize = _text.fontSize;
    //     }
    // }

    // void OnDisable()
    // {
    //     if (isText)
    //     {
    //         _text.fontSize = _originalFontSize;
    //     }
    // }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Play("SFX/NightOfTheSpook");
        enabled = false;
    }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if (isText)
    //     {
    //         _growthAnimation.Kill();
    //         _text.DOFontSize(_originalFontSize, duration);
    //     }
    // }
}
