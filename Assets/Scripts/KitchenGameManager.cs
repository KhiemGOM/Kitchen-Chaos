using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public class GameStateChangeEventArgs : EventArgs
    {
        public GameStateChangeEventArgs(GameState state)
        {
            this.state = state;
        }

        public readonly GameState state;
    }


    public enum GameState
    {
        BeforeStart,
        Tutorial,
        Countdown,
        Playing,
        Paused,
        GameOver,
        Option
    }

    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler<GameStateChangeEventArgs> OnGameStateChanged;
    [SerializeField] private float maxPlayingTime = 60f;
    private GameState state;
    public GameState prevStateBeforePause;
    private float countdownTimer = 3f;
    private float playingTimer;
    private bool firstUpdateClock = true;
    public int Score { get; set; }

    public GameState State
    {
        get => state;
        set
        {
            state = value;
            OnGameStateChanged?.Invoke(this, new GameStateChangeEventArgs(value));
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        State = GameState.BeforeStart;
        GameInput.Instance.OnPause += OnPause;
    }

    private void OnPause(object sender, EventArgs e)
    {
        switch (State)
        {
            case GameState.Playing:
            case GameState.BeforeStart:
            case GameState.Countdown:
                prevStateBeforePause = State;
                State = GameState.Paused;
                Time.timeScale = 0f;
                break;
            case GameState.Paused:
                State = prevStateBeforePause;
                Time.timeScale = 1f;
                break;
            case GameState.Option:
                State = GameState.Paused;
                break;
            case GameState.GameOver: return;
            case GameState.Tutorial:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.Countdown:
                countdownTimer -= Time.deltaTime;
                CountdownUI.Instance.Text = Mathf.CeilToInt(countdownTimer).ToString();
                if (countdownTimer <= 0f)
                {
                    State = GameState.Playing;
                    playingTimer = maxPlayingTime;
                }

                break;
            case GameState.BeforeStart:
                Time.timeScale = 1f;
                State = GameState.Tutorial;
                break;
            case GameState.Playing:
                playingTimer -= Time.deltaTime;
                GameClockUI.Instance.Fill = playingTimer / maxPlayingTime;
                //If 1/4 time left, start changing to red color for 1/16 of the time then stay at red
                if (playingTimer <= maxPlayingTime / 4f && playingTimer > maxPlayingTime / 4f - 1)
                {
                    GameClockUI.Instance.RedColor =
                        Mathf.InverseLerp(maxPlayingTime / 4f, maxPlayingTime / 4f - 1, playingTimer);
                }

                if (playingTimer <= maxPlayingTime * 3f / 16f && firstUpdateClock)
                {
                    GameClockUI.Instance.PlayClockTick();
                    GameClockUI.Instance.RedColor = 1f;
                    firstUpdateClock = false;
                }

                if (playingTimer <= 0f)
                {
                    SFXManager.Instance.PlaySound(SFXManager.SFXType.TimesUpBellRing, Vector3.zero);
                    GameClockUI.Instance.StopClockTick();
                    State = GameState.GameOver;
                }

                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
            case GameState.Option:
                break;
            case GameState.Tutorial:
                if (GameInput.Instance.IsInteractKeyPressed())
                {
                    State = GameState.Countdown;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool IsPlaying()
    {
        return State == GameState.Playing;
    }
}