using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClockUI : MonoBehaviour
{
    [SerializeField] private Image clockImage;
    [SerializeField] private AudioSource clockTickAudioSource;
    public static GameClockUI Instance { get; private set; }
    public float Volume { get; set; }
    private Color initColor;

    public float Fill
    {
        set => clockImage.fillAmount = value;
    }

    public void PlayClockTick()
    {
        clockTickAudioSource.volume = Volume;
        if (!clockTickAudioSource.isPlaying)
            clockTickAudioSource.Play();
    }

    public void StopClockTick()
    {
        clockTickAudioSource.Stop();
    }

    public float RedColor
    {
        set => clockImage.color = Color.Lerp(initColor, Color.red, value);
    }

    private void Start()
    {
        initColor = clockImage.color;
    }

    private void Awake()
    {
        Instance = this;
    }
}