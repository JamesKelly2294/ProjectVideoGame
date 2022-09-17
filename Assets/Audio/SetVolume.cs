using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    Music,
    SFX,
    Environment
}

public class SetVolume : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioType audioType;

    public void SetLevel(float sliderValue)
    {
        float trueValue = Mathf.Log10(sliderValue * 20);
        Debug.Log("sliderValue=" + sliderValue);
        Debug.Log("trueValue=" + trueValue);
        switch(audioType)
        {
            case AudioType.Music:
                audioManager.SetMusicVol(trueValue);
                break;
            case AudioType.SFX:
                audioManager.SetSFXVol(trueValue);
                break;
            case AudioType.Environment:
                audioManager.SetEnvironmentVol(trueValue);
                break;
            default:
                break;
        }
    }
}
