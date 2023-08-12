using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimation : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;
    private readonly int animationID = Animator.StringToHash("OpenClose");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        containerCounter.PlayerPickUp += OnPlayerPickUp;
    }

    private void OnPlayerPickUp(object sender, EventArgs e)
    {
        animator.SetTrigger(animationID);
    }
}