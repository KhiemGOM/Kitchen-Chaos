using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    [SerializeField] private GameObject onVisual;
    [SerializeField] private StoveCounter stoveCounter;

    private void Awake()
    {
        stoveCounter.OnFryingStateChanged += OnFryingStateChanged;
    }

    private void OnFryingStateChanged(object sender, StoveCounter.StoveCounterEventArgs e)
    {
        if (e.state == StoveCounter.FryingState.None)
        {
            particle.SetActive(false);
            onVisual.SetActive(false);
        }
        else
        {
            particle.SetActive(true);
            onVisual.SetActive(true);
        }
    }
}