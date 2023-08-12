using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialUI;
    private void Start()
    {
        KitchenGameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(object sender, KitchenGameManager.GameStateChangeEventArgs e)
    {
        tutorialUI.SetActive(e.state == KitchenGameManager.GameState.Tutorial);
    }
}