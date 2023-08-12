using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    private float _volume = 1f;

    public float Volume
    {
        get => _volume;
        set
        {
            _volume = value;
            StoveCounter.SFXVolume = value;
            GameClockUI.Instance.Volume = value;
        }
    }

    public enum SFXType
    {
        Chop,
        DeliveryFail,
        DeliverySuccess,
        Footstep,
        ObjectDrop,
        ObjectPickUp,
        PanSizzle,
        Trash,
        Warning,
        DeliveryRing,
        TimesUpBellRing
    }

    [SerializeField] private AllSFXAudioClipSO sfxAudioClips;
    private Vector3 mainCamPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Camera.main != null) mainCamPosition = Camera.main.transform.position;
    }

    public AudioClip SFXTypeToAudioClip(SFXType sfxType)
    {
        var clip = sfxAudioClips.sfxTypeToAudioClips.Find(n => n.from == sfxType);
        return clip.to[Random.Range(0, clip.to.Length)];
    }

    public AudioClip[] GetAudioClipsFromSFXType(SFXType sfxType)
    {
        var clip = sfxAudioClips.sfxTypeToAudioClips.Find(n => n.from == sfxType);
        return clip.to;
    }

    public void PlaySound(SFXType sfxType, Vector3 position, float volume = 1f)
    {
        position = sfxType == SFXType.Footstep ? Vector3.Lerp(position, mainCamPosition, 0.7f) : position;
        var clip = sfxAudioClips.sfxTypeToAudioClips.Find(n => n.from == sfxType);
        PlaySound(clip.to[Random.Range(0, clip.to.Length)], position, volume);
    }


    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        position = position == Vector3.zero ? mainCamPosition : Vector3.Lerp(position, mainCamPosition, 0.7f);
        AudioSource.PlayClipAtPoint(clip, position, volume * Volume);
    }
}