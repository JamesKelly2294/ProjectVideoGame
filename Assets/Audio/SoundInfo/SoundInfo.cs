using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundInfo", menuName = "Sound Info", order = 1)]
public class SoundInfo : ScriptableObject
{
    public string id;

    public AudioMixerGroup audioMixerGroup;
    public AudioClip[] audioClips;
}
