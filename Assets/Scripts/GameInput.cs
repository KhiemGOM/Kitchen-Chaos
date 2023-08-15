using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public enum KeyBindType
    {
        Interact,
        InteractAlt,
        Pause,
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Sprint,
    }

    public event EventHandler OnInteract;
    public event EventHandler OnInteractAlt;
    public event EventHandler OnPause;
    private PlayerInputAction inputAction;
    private TextMeshProUGUI tempKeyBindText;
    private const string GAME_INPUT_MAPPING = "GameInputMapping";
    public static GameInput Instance { get; private set; }

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        Instance = this;
        inputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(GAME_INPUT_MAPPING, inputAction.SaveBindingOverridesAsJson()));
        inputAction.Player.Enable();

        inputAction.Player.Interact.performed += OnInteractPerformed;
        inputAction.Player.InteractAlt.performed += OnInteractAltPerformed;
        inputAction.Player.Pause.performed += OnPausePerformed;
    }

    public void ChangeKeyBind(KeyBindType keyBindType, Action onKeyBindChanged, TextMeshProUGUI keyBindText)
    {
        int index;
        inputAction.Player.Disable();
        (keyBindType switch
        {
            KeyBindType.Interact => SetIndex(inputAction.Player.Interact, out index),
            KeyBindType.InteractAlt => SetIndex(inputAction.Player.InteractAlt, out index),
            KeyBindType.Sprint => SetIndex(inputAction.Player.Sprint, out index),
            KeyBindType.Pause => SetIndex(inputAction.Player.Pause, out index),
            KeyBindType.MoveUp => SetIndex(inputAction.Player.Move, out index, 1),
            KeyBindType.MoveDown => SetIndex(inputAction.Player.Move, out index, 2),
            KeyBindType.MoveLeft => SetIndex(inputAction.Player.Move, out index, 3),
            KeyBindType.MoveRight => SetIndex(inputAction.Player.Move, out index, 4),
            _ => throw new ArgumentOutOfRangeException()
        }).PerformInteractiveRebinding(index).OnComplete(_ =>
        {
            inputAction.Player.Enable();
            onKeyBindChanged();
            keyBindText.text = GetKeyBindText(keyBindType);
            PlayerPrefs.SetString(GAME_INPUT_MAPPING, inputAction.SaveBindingOverridesAsJson());
        }).Start();
    }

    private static InputAction SetIndex(InputAction action, out int index, int val = -1)
    {
        index = val;
        return action;
    }

    public string GetKeyBindText(KeyBindType keyBindType)
    {
        return keyBindType switch
        {
            KeyBindType.Interact => inputAction.Player.Interact.GetBindingDisplayString(),
            KeyBindType.InteractAlt => inputAction.Player.InteractAlt.GetBindingDisplayString(),
            KeyBindType.Pause => inputAction.Player.Pause.GetBindingDisplayString(),
            KeyBindType.MoveUp => inputAction.Player.Move.bindings[1].ToDisplayString(),
            KeyBindType.MoveDown => inputAction.Player.Move.bindings[2].ToDisplayString(),
            KeyBindType.MoveLeft => inputAction.Player.Move.bindings[3].ToDisplayString(),
            KeyBindType.MoveRight => inputAction.Player.Move.bindings[4].ToDisplayString(),
            KeyBindType.Sprint => inputAction.Player.Sprint.GetBindingDisplayString(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteractAltPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAlt?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteractPerformed(InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMovementVectorNormalized()
    {
        var moveVector = inputAction.Player.Move.ReadValue<Vector2>();

        return new Vector3(moveVector.x, 0, moveVector.y).normalized;
    }

    public bool IsSprintKeyPressed()
    {
        return inputAction.Player.Sprint.IsPressed();
    }

    public bool IsInteractKeyPressed()
    {
        return inputAction.Player.Interact.IsPressed();
    }
}