using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    [SerializeField]
    private string ButtonHighlightSoundID;
    [SerializeField]
    private string ButtonPressedSoundID;
    [SerializeField]
    private string ButtonReleasedSoundID;

    private AudioManager _am;

    public void Start()
    {
        _am = AudioManager.Instance;
    }

    public void PlayButtonHighlightedSound()
    {
        if (_am)
        {
            _am.Play(ButtonHighlightSoundID);
        }
    }

    public void PlayButtonPressedSound()
    {
        if(_am)
        {
            _am.Play(ButtonPressedSoundID);
        }
    }

    public void PlayButtonReleasedSound()
    {
        if (_am)
        {
            _am.Play(ButtonReleasedSoundID);
        }
    }
}
