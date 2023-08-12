using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterProgressBar : MonoBehaviour
{
    public Color Color
    {
        get => fillImage.color;
        set => fillImage.color = value;
    }
    [SerializeField] private Canvas progressBar;
    private Image fillImage;


    public void Awake()
    {
        fillImage = GetComponent<Image>();
        SetProgress(0);
    }
    public void SetProgress(float progress)
    {
        fillImage.fillAmount = progress;
        progressBar.enabled = progress is > 0 and < 1;
    }
}