using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;
    private int id;
    private string prevText = "";
    private const string POP_UP_NAME = "PopUp";
    private AudioClip[] warningAudioClips;
    public static CountdownUI Instance { get; private set; }

    public string Text
    {
        get => text.text;
        set
        {
            text.text = value;
            if (prevText == value) return;
            animator.SetTrigger(id);
            prevText = value;
            SFXManager.Instance.PlaySound(value == "0" ? warningAudioClips[1] : warningAudioClips[0], Vector3.zero);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        warningAudioClips = SFXManager.Instance.GetAudioClipsFromSFXType(SFXManager.SFXType.Warning);
        id = Animator.StringToHash(POP_UP_NAME);
        KitchenGameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(object sender, KitchenGameManager.GameStateChangeEventArgs e)
    {
        text.gameObject.SetActive(e.state is KitchenGameManager.GameState.Countdown);
    }
}