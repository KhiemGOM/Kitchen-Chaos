using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AllSFXAudioClipSO : ScriptableObject
{
    [Serializable]
    public struct SFXTypeToAudioClip
    {
        public SFXManager.SFXType from;
        public AudioClip[] to;

    }
    public List<SFXTypeToAudioClip> sfxTypeToAudioClips;
}