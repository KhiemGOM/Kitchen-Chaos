using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterAnimation : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;
    private readonly int animationID = Animator.StringToHash("Cut");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cuttingCounter.PlayerCut += OnPlayerCut;
    }

    private void OnPlayerCut(object sender, EventArgs e)
    {
        animator.SetTrigger(animationID);
    }
}