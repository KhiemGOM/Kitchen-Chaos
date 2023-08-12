using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderUI : MonoBehaviour
{
    public event System.Action<float> OnVolumeChanged;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI volumeText;
    public float Volume
    {
        get => slider.value;
        set
        {
            slider.value = value;
            volumeText.text = $"{(int)(value * 100)}%";
        }
    }

    private void Awake()
    {
        slider.onValueChanged.AddListener(val =>
        {
            OnVolumeChanged?.Invoke(val);
            Volume = val;
        });
    }
}