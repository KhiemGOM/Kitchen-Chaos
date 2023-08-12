using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private AudioSource gameMusic;
    [SerializeField] private VolumeSliderUI musicSlider;
    [SerializeField] private VolumeSliderUI sfxSlider;
    [SerializeField] private Button backButton;
    private const string GAME_MUSIC_VOLUME_KEY = "GameMusicVolume";
    private const string GAME_SFX_VOLUME_KEY = "GameSFXVolume";
    public static OptionMenuUI Instance { get; private set; }

    private float GameMusicVolume
    {
        get => gameMusic.volume;
        set
        {
            gameMusic.volume = value;
            musicSlider.Volume = value;
            PlayerPrefs.SetFloat(GAME_MUSIC_VOLUME_KEY, value);
        }
    }

    private float GameSFXVolume
    {
        get => SFXManager.Instance.Volume;
        set
        {
            SFXManager.Instance.Volume = value;
            sfxSlider.Volume = value;
            PlayerPrefs.SetFloat(GAME_SFX_VOLUME_KEY, value);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void SFXSliderOnVolumeChanged(float obj)
    {
        GameSFXVolume = obj;
    }

    private void MusicSliderOnVolumeChanged(float obj)
    {
        GameMusicVolume = obj;
    }

    private void Start()
    {
        GameMusicVolume = PlayerPrefs.GetFloat(GAME_MUSIC_VOLUME_KEY, 1f);
        GameSFXVolume = PlayerPrefs.GetFloat(GAME_SFX_VOLUME_KEY, 1f);
        KitchenGameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        musicSlider.OnVolumeChanged += MusicSliderOnVolumeChanged;
        sfxSlider.OnVolumeChanged += SFXSliderOnVolumeChanged;
        backButton.onClick.AddListener(() => { KitchenGameManager.Instance.State = KitchenGameManager.GameState.Paused; });
    }

    private void OnGameStateChanged(object sender, KitchenGameManager.GameStateChangeEventArgs e)
    {
        if (optionMenu != null)
            optionMenu.SetActive(e.state == KitchenGameManager.GameState.Option);
    }
}