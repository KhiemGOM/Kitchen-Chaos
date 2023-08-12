using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyBindingTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyBindText;
    [SerializeField] private BaseCounter counter;

    private void Start()
    {
        Player.Instance.OnSelect += OnSelect;
        Player.Instance.OnInteract += OnInteract;
    }

    private void OnInteract(object sender, EventArgs e)
    {
        keyBindText.text = counter.GetInteractionName();
    }

    private void OnSelect(object sender, Player.SelectEventArgs e)
    {
        keyBindText.text = counter.GetInteractionName();
    }
}