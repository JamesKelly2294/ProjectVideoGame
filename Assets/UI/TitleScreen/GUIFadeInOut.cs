using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GUIFadeInOut : MonoBehaviour
{
    public CanvasGroup target;
    
    public void EnableAndFadeIn(float duration)
    {
        Fade(0, 1, duration);
    }

    public void FadeOutAndDisable(float duration)
    {
        Fade(1, 0, duration, disableGameObjectOnComplete: true);
    }

    private void Fade(float startOpacity, float endOpacity, float duration, bool disableGameObjectOnComplete = false)
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        target.alpha = startOpacity;
        var fade = target.DOFade(endOpacity, duration);
        if(disableGameObjectOnComplete)
        {
            fade.OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}