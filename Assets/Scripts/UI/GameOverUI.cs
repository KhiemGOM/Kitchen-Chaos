using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    public string Text
    {
        get => scoreText.text;
        set => scoreText.text = value;
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        playAgainButton.onClick.AddListener(() => SceneLoader.LoadScene(SceneLoader.Scene.GameScene));
        mainMenuButton.onClick.AddListener(() => SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene));
    }

    private void OnGameStateChanged(object sender, KitchenGameManager.GameStateChangeEventArgs e)
    {
        gameOverUI.SetActive(e.state == KitchenGameManager.GameState.GameOver);
        if (KitchenGameManager.Instance.State == KitchenGameManager.GameState.GameOver)
        {
            Text = KitchenGameManager.Instance.Score.ToString();
        }
    }
}