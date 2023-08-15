using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionButton;
    public static PauseMenuUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.State = KitchenGameManager.Instance.prevStateBeforePause;
            Time.timeScale = 1f;
        });
        mainMenuButton.onClick.AddListener(() => { SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene); });
        optionButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.State = KitchenGameManager.GameState.Option;
        });
    }

    private void OnGameStateChanged(object sender, KitchenGameManager.GameStateChangeEventArgs e)
    {
        pauseMenu.SetActive(e.state == KitchenGameManager.GameState.Paused);
    }
}