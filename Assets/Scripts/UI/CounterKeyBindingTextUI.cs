using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CounterKeyBindingTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private TextMeshProUGUI interactKeyBindText;
    [SerializeField] private TextMeshProUGUI interactAltKeyBindText;
    [SerializeField] private BaseCounter counter;
    private bool isInteractAltKeyBindTextNotNull;

    private void Start()
    {
        isInteractAltKeyBindTextNotNull = interactAltKeyBindText != null;
        Player.Instance.OnSelect += OnSelect;
        Player.Instance.OnInteract += OnInteract;
    }

    private void OnInteract(object sender, EventArgs e)
    {
        interactKeyBindText.text = GameInput.Instance.GetKeyBindText(GameInput.KeyBindType.Interact);
        if (isInteractAltKeyBindTextNotNull)
        {
            interactAltKeyBindText.text = GameInput.Instance.GetKeyBindText(GameInput.KeyBindType.InteractAlt);
        }

        interactionText.text = counter.GetInteractionName();
    }

    private void OnSelect(object sender, Player.SelectEventArgs e)
    {
        interactKeyBindText.text = GameInput.Instance.GetKeyBindText(GameInput.KeyBindType.Interact);
        if (isInteractAltKeyBindTextNotNull)
        {
            interactAltKeyBindText.text = GameInput.Instance.GetKeyBindText(GameInput.KeyBindType.InteractAlt);
        }

        interactionText.text = counter.GetInteractionName();
    }
}