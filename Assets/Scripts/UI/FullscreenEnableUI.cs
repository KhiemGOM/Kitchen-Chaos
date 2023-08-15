using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FullscreenEnableUI : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle;
    private const string GAME_FULLSCREEN = "GameFullscreen";

    private void Start()
    {
        fullscreenToggle.isOn = PlayerPrefs.GetInt(GAME_FULLSCREEN, 1) == 1;
        Screen.fullScreenMode = PlayerPrefs.GetInt(GAME_FULLSCREEN, 1) == 1
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;
        fullscreenToggle.onValueChanged.AddListener(fullscreen =>
        {
            Screen.fullScreenMode = fullscreen
                ? FullScreenMode.FullScreenWindow
                : FullScreenMode.Windowed;
            PlayerPrefs.SetInt(GAME_FULLSCREEN, fullscreen ? 1 : 0);
        });
    }
}